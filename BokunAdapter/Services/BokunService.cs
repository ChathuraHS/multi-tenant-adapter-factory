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

                    Availabilities = new List<ProductAvailabilityDto>() // Bokun needs another endpoint for real availability
                });
            }

            return result;

        }
    }
}
