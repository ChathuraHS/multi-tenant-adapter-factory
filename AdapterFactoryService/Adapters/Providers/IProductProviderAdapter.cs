using AdapterFactoryService.Dtos;
using BokunAdapter.Dto;

namespace AdapterFactoryService.Adapters.Providers
{
    public interface IProductProviderAdapter
    {
        Task<ProductResponseDto> GetProductAsync(string externalId);

        Task<List<ProductSyncDto>> GetAllProductsAsync();

        Task<BokunAvailabilityDto?> GetAvailabilityAsync(long productId, DateTime date);

        //return the raw JSON response from the provider so the factory can forward it unchanged
        Task<string?> GetAvailabilityRawAsync(long productId, DateTime date);
    }
}
