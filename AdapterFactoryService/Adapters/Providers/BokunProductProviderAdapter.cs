//The bridge between the Adapter Factory and the Bokun Adapter microservice.

using AdapterFactoryService.Dtos;
using BokunAdapter.Dto;

namespace AdapterFactoryService.Adapters.Providers
{
    public class BokunProductProviderAdapter : IProductProviderAdapter
    {
        //Stores the HttpClient that will be used to call the Bokun Adapter microservice.
        private readonly HttpClient _http;

        //ASP.NET injects an IHttpClientFactory 
        public BokunProductProviderAdapter(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("BokunAdapter");
            //Asks the client factory to create an HttpClient configured to communicate with the Bokun Adapter microservice. 
            //http://bokun-adapter (The client is configured in program.cs with Base URL, Headers etc)
        }

        public async Task<ProductResponseDto> GetProductAsync(string externalId)
        {
            //Expect JSON response and automatically convert it into ProductResponseDto
            //Send a GET request to the Bokun Adapter microservice:
            var res = await _http.GetFromJsonAsync<ProductResponseDto>(
                $"/api/bokun/products/{externalId}"
            );

            return res!; //! tells the compiler result cant be null.
        }

        public async Task<List<ProductSyncDto>> GetAllProductsAsync()
        {
            var res = await _http.GetFromJsonAsync<List<ProductSyncDto>>(
                "/api/bokun/products"
            );

            return res ?? new List<ProductSyncDto>();
        }

        public async Task<BokunAvailabilityDto?> GetAvailabilityAsync(long productId, DateTime date)
        {
            var res = await _http.GetFromJsonAsync<BokunAvailabilityDto?>(
                $"/api/bokun/products/{productId}/availability?date={date:yyyy-MM-dd}"
            );
            return res;
        }
        // New: return raw JSON so the AdapterFactory can forward exactly what BokunAdapter returned.
        public async Task<string?> GetAvailabilityRawAsync(long productId, DateTime date)
        {
            var url = $"/api/bokun/products/{productId}/availability?date={date:yyyy-MM-dd}";
            var res = await _http.GetAsync(url);

            if (!res.IsSuccessStatusCode)
                return null;

            var json = await res.Content.ReadAsStringAsync();

            // Treat empty/empty-object/array/null as no availability
            if (string.IsNullOrWhiteSpace(json))
                return null;

            var trimmed = json.Trim();
            if (trimmed == "null" || trimmed == "{}" || trimmed == "[]")
                return null;

            return json;
        }
    }
}
