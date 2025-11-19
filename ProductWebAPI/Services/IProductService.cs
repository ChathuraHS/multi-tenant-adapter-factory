using Microsoft.AspNetCore.Mvc;
using ProductWebAPI.Dto;
using ProductWebAPI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProductWebAPI.Services
{
    public interface IProductService
    {
        Task<List<ProductResponseDto>> GetAllAsync();
        Task<ProductResponseDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(ProductRequestDto dto);
        Task<bool> UpdateAsync(int id, ProductRequestDto dto);
        Task<bool> DeleteAsync(int id);
        Task<int> SyncFromProviderAsync(string provider);

        Task<BokunAvailabilityDto?> GetAvailabilityFromProviderAsync(
             string provider,
             long productId,
             DateTime date);
    }

}
