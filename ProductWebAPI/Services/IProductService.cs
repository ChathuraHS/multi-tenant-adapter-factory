using ProductWebAPI.Dto;

namespace ProductWebAPI.Services
{
    public interface IProductService
    {
        Task<List<ProductResponseDto>> GetAllAsync();
        Task<ProductResponseDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(ProductRequestDto dto);
        Task<bool> UpdateAsync(int id, ProductRequestDto dto);
        Task<bool> DeleteAsync(int id);
        //Task<int> SyncFromBokunAsync();
        //Task SyncFromProviderAsync(string provider);
        Task<int> SyncFromProviderAsync(string provider);
    }

}
