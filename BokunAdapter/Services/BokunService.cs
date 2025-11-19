namespace BokunAdapter.Services

{
    using BokunAdapter.Dto;
    using BokunAdapter.Models;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.Json;
    public class BokunService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public BokunService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task<List<ProductSyncDto>> FetchProductsAsync()
        {
            var accessKey = _config["Bokun:AccessKey"];
            var secretKey = _config["Bokun:SecretKey"];
            var baseUrl = _config["Bokun:BaseUrl"];

            string path = "/activity.json/search";
            string method = "POST";

            string utcDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            string stringToSign = utcDate + accessKey + method + path;

            // HMAC SHA1
            using var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(secretKey));
            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));

            // Add headers
            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("X-Bokun-Date", utcDate);
            _http.DefaultRequestHeaders.Add("X-Bokun-AccessKey", accessKey);
            _http.DefaultRequestHeaders.Add("X-Bokun-Signature", signature);

            var response = await _http.PostAsync(baseUrl + path,
                new StringContent("{}", Encoding.UTF8, "application/json"));

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<BokunResponse>(json);

            return MapBokunToInternal(data);
        }

        public async Task<ProductAvailabilityDto?> GetAvailabilityAsync(long productId, DateTime selectedDate)
        {
            var accessKey = _config["Bokun:AccessKey"];
            var secretKey = _config["Bokun:SecretKey"];
            var baseUrl = _config["Bokun:BaseUrl"];

            string start = selectedDate.ToString("yyyy-MM-dd");
            string end = start;

            string path = $"/activity.json/{productId}/availabilities?start={start}&end={end}";
            string method = "GET";

            string utcDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            string stringToSign = utcDate + accessKey + method + path;

            using var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(secretKey));
            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));

            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("X-Bokun-Date", utcDate);
            _http.DefaultRequestHeaders.Add("X-Bokun-AccessKey", accessKey);
            _http.DefaultRequestHeaders.Add("X-Bokun-Signature", signature);

            var response = await _http.GetAsync(baseUrl + path);
            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Raw JSON from Bokun:");
            Console.WriteLine(json);

            // --- FIX START ---
            if (string.IsNullOrWhiteSpace(json))
                return null;

            var trimmed = json.Trim();

            // Bokun sometimes returns {} or null
            if (trimmed == "{}" || trimmed == "null")
                return null;

            // Must be array; if it isn't, treat as empty availability
            if (!trimmed.StartsWith("["))
                return null;
            // --- FIX END ---

            var data = JsonSerializer.Deserialize<List<BokunAvailabilityDto>>(json);

            return MapAvailability(data);
        }



        private List<ProductSyncDto> MapBokunToInternal(BokunResponse data)
        {
            var result = new List<ProductSyncDto>();

            foreach (var item in data.items)
            {
                result.Add(new ProductSyncDto
                {
                    ExternalId = item.id,
                    Provider = "Bokun",
                    ProductType = "DAY_TOUR_OR_ACTIVITY",
                    Name = item.title,
                    Description = item.durationText ?? "No description",
                    CategoryId = 1,
                    Price = item.price,
                    Currency = "USD",
                    Location = item.vendor?.title ?? "N/A",
                    Images = new List<string> { item.keyPhoto?.originalUrl },

                    Attributes = new List<ProductAttributeDto>
                {
                    new ProductAttributeDto { Name = "Duration", Value = item.durationText },
                    new ProductAttributeDto { Name = "Difficulty", Value = item.difficultyLevel }
                },

                    Availabilities = new List<ProductAvailabilityDto>() // Need to remove this and implement it real time
                });
            }

            return result;

        }

        private ProductAvailabilityDto? MapAvailability(List<BokunAvailabilityDto> items)
        {
            if (items == null || items.Count == 0)
                return null;

            var a = items[0];

            decimal unitPrice = 0;
            string currency = "USD";

            // Take first rate & first pricePerCategoryUnit as default
            if (a.PricesByRate != null && a.PricesByRate.Count > 0 &&
                a.PricesByRate[0].PricePerCategoryUnit != null &&
                a.PricesByRate[0].PricePerCategoryUnit.Count > 0)
            {
                unitPrice = a.PricesByRate[0].PricePerCategoryUnit[0].Amount.Value;
                currency = a.PricesByRate[0].PricePerCategoryUnit[0].Amount.Currency;
            }

            int availableUnits = a.UnlimitedAvailability ? int.MaxValue : a.AvailabilityCount;

            return new ProductAvailabilityDto
            {
                Date = a.Date,
                AvailableUnits = availableUnits,
                UnitPrice = unitPrice,
                Currency = currency
            };
        }

        public async Task<CheckoutOptionsResponse> GetCheckoutOptionsAsync(long activityId)
        {
            string path = $"/checkout.json/options/booking-request";
            string method = "POST";

            var body = new
            {
                directBooking = new { activityId, startTimeId = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"), quantity = 1 },
                source = "DIRECT_REQUEST"
            };

            var response = await SendBokunRequestAsync(path, method, body);
            return JsonSerializer.Deserialize<CheckoutOptionsResponse>(response); // ✅ single object
        }


        public async Task<string> SubmitCheckoutAsync(CheckoutRequestDto request)
        {
            string path = "/checkout.json/submit";
            string method = "POST";

            var response = await SendBokunRequestAsync(path, method, request);
            return response; // raw JSON with booking details, payment URLs, etc.
        }

        // Shared helper
        private async Task<string> SendBokunRequestAsync(string path, string method, object body)
        {
            var accessKey = _config["Bokun:AccessKey"];
            var secretKey = _config["Bokun:SecretKey"];
            var baseUrl = _config["Bokun:BaseUrl"];

            string utcDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            string stringToSign = utcDate + accessKey + method.ToUpper() + path;

            using var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(secretKey));
            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));

            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("X-Bokun-Date", utcDate);
            _http.DefaultRequestHeaders.Add("X-Bokun-AccessKey", accessKey);
            _http.DefaultRequestHeaders.Add("X-Bokun-Signature", signature);

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var httpResponse = await _http.PostAsync(baseUrl + path, content);
            var json = await httpResponse.Content.ReadAsStringAsync();

            Console.WriteLine(json); // debug raw response

            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception($"Bokun error: {httpResponse.StatusCode}, {json}");

            return json;
        }





    }
}
