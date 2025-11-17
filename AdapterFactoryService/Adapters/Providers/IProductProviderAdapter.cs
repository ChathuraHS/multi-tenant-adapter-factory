using AdapterFactoryService.Dtos;
using BokunAdapter.Dto;

namespace AdapterFactoryService.Adapters.Providers
{
    public interface IProductProviderAdapter
    {
        Task<ProductResponseDto> GetProductAsync(string externalId);

        Task<List<ProductSyncDto>> GetAllProductsAsync();
    }
}
