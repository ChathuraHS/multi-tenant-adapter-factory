using AdapterFactoryService.Dtos;
using BokunAdapter.Dto;

namespace AdapterFactoryService.Adapters.Providers
{
    public class BokunProductProviderAdapter : IProductProviderAdapter
    {
        private readonly HttpClient _http;

        public BokunProductProviderAdapter(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("BokunAdapter");
        }

        public async Task<ProductResponseDto> GetProductAsync(string externalId)
        {
            var res = await _http.GetFromJsonAsync<ProductResponseDto>(
                $"/api/bokun/products/{externalId}"
            );

            return res!;
        }

        public async Task<List<ProductSyncDto>> GetAllProductsAsync()
        {
            var res = await _http.GetFromJsonAsync<List<ProductSyncDto>>(
                "/api/bokun/products"
            );

            return res ?? new List<ProductSyncDto>();
        }
    }
}
