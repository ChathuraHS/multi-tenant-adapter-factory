using BokunAdapter.Dto;
using Microsoft.EntityFrameworkCore;
using ProductWebAPI.Dto;
using ProductWebAPI.Models;

namespace ProductWebAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductDbContext _db;
        private readonly IHttpClientFactory _clientFactory;

        public ProductService(ProductDbContext db, IHttpClientFactory clientFactory)
        {
            _db = db;
            _clientFactory = clientFactory;
        }

        // ---------------- GET ALL ----------------
        public async Task<List<ProductResponseDto>> GetAllAsync()
        {
            var products = await _db.Products
                .Include(p => p.Category)
                .Include(p => p.TourImages)
                .Include(p => p.Attributes)
                .Include(p => p.TourAvailabilities)
                .ToListAsync();

            return products.Select(MapToDto).ToList();
        }

        // ---------------- GET ONE ----------------
        public async Task<ProductResponseDto?> GetByIdAsync(int id)
        {
            var p = await _db.Products
                .Include(x => x.Category)
                .Include(x => x.TourImages)
                .Include(x => x.Attributes)
                .Include(x => x.TourAvailabilities)
                .FirstOrDefaultAsync(x => x.ProductId == id);

            return p == null ? null : MapToDto(p);
        }

        // ---------------- CREATE ----------------
        public async Task<int> CreateAsync(ProductRequestDto dto)
        {
            var product = MapToEntity(dto);

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return product.ProductId;
        }

        // ---------------- UPDATE ----------------
        public async Task<bool> UpdateAsync(int id, ProductRequestDto dto)
        {
            var existing = await _db.Products
                .Include(p => p.Attributes)
                .Include(p => p.TourAvailabilities)
                .Include(p => p.TourImages)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (existing == null)
                return false;

            // Update main fields
            existing.ExternalId = dto.ExternalId;
            existing.Provider = dto.Provider;
            existing.ProductType = dto.ProductType;
            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.CategoryId = dto.CategoryId;
            existing.Price = dto.Price;
            existing.Currency = dto.Currency;
            existing.Location = dto.Location;
            existing.UpdatedAt = DateTime.UtcNow;

            // Replace children
            _db.Attributes.RemoveRange(existing.Attributes);
            existing.Attributes = dto.Attributes?
                .Select(a => new ProductAttribute { ProductId = id, Name = a.Name, Value = a.Value })
                .ToList();

            _db.ProductAvailabilities.RemoveRange(existing.TourAvailabilities);
            existing.TourAvailabilities = dto.Availabilities?
                .Select(a => new ProductAvailability { ProductId = id, Date = a.Date, AvailableUnits = a.AvailableUnits, Price = a.Price })
                .ToList();

            _db.ProductImages.RemoveRange(existing.TourImages);
            existing.TourImages = dto.Images?
                .Select(img => new ProductImage { ProductId = id, ImageUrl = img })
                .ToList();

            await _db.SaveChangesAsync();
            return true;
        }

        // ---------------- DELETE ----------------
        public async Task<bool> DeleteAsync(int id)
        {
            var p = await _db.Products.FindAsync(id);
            if (p == null) return false;

            _db.ProductImages.RemoveRange(_db.ProductImages.Where(i => i.ProductId == id));
            _db.Attributes.RemoveRange(_db.Attributes.Where(a => a.ProductId == id));
            _db.ProductAvailabilities.RemoveRange(_db.ProductAvailabilities.Where(a => a.ProductId == id));

            _db.Products.Remove(p);

            await _db.SaveChangesAsync();
            return true;
        }

        //// ---------------- SYNC FROM BOKUN ----------------
        //public async Task<int> SyncFromBokunAsync()
        //{
        //    var http = _clientFactory.CreateClient();
        //    var products = await http.GetFromJsonAsync<List<ProductSyncDto>>(
        //        "http://bokun-adapter/api/bokun/products"
        //    );

        //    if (products == null || products.Count == 0)
        //        return 0;

        //    int count = 0;

        //    foreach (var p in products)
        //    {
        //        var existing = await _db.Products
        //            .Include(x => x.Attributes)
        //            .Include(x => x.TourImages)
        //            .Include(x => x.TourAvailabilities)
        //            .FirstOrDefaultAsync(x => x.ExternalId == p.ExternalId && x.Provider == "Bokun");

        //        if (existing == null)
        //        {
        //            await _db.Products.AddAsync(MapSyncToEntity(p));
        //        }
        //        else
        //        {
        //            // Same update logic reused from UpdateAsync()
        //            existing.Name = p.Name;
        //            existing.Description = p.Description;
        //            existing.Price = p.Price;
        //            existing.Currency = p.Currency;
        //            existing.CategoryId = p.CategoryId;
        //            existing.UpdatedAt = DateTime.UtcNow;

        //            _db.Attributes.RemoveRange(existing.Attributes);
        //            existing.Attributes = p.Attributes?
        //                .Select(a => new ProductAttribute { ProductId = existing.ProductId, Name = a.Name, Value = a.Value })
        //                .ToList();

        //            _db.ProductImages.RemoveRange(existing.TourImages);
        //            existing.TourImages = p.Images?
        //                .Select(i => new ProductImage { ProductId = existing.ProductId, ImageUrl = i })
        //                .ToList();

        //            _db.ProductAvailabilities.RemoveRange(existing.TourAvailabilities);
        //            existing.TourAvailabilities = p.Availabilities?
        //                .Select(a => new ProductAvailability { ProductId = existing.ProductId, Date = a.Date, AvailableUnits = a.AvailableUnits, Price = a.Price })
        //                .ToList();
        //        }

        //        count++;
        //    }

        //    await _db.SaveChangesAsync();
        //    return count;
        //}
        public async Task<int> SyncFromProviderAsync(string provider)
        {
            // normalize provider name
            provider = provider.ToLower();

            // Use Adapter Factory instead of direct microservice calls
            var client = _clientFactory.CreateClient("AdapterFactory");

            // FIX: controller name is "Adapter" so route is api/Adapter/{provider}/products
            var endpoint = $"api/Adapter/{provider}/products";

            var products = await client.GetFromJsonAsync<List<ProductSyncDto>>(endpoint);

            if (products == null || products.Count == 0)
                return 0;

            int count = 0;

            foreach (var p in products)
            {
                var existing = await _db.Products
                    .Include(x => x.Attributes)
                    .Include(x => x.TourImages)
                    .Include(x => x.TourAvailabilities)
                    .FirstOrDefaultAsync(x => x.ExternalId == p.ExternalId && x.Provider == provider);

                if (existing == null)
                {
                    var newProd = MapSyncToEntity(p);
                    newProd.Provider = provider;
                    await _db.Products.AddAsync(newProd);
                }
                else
                {
                    existing.Name = p.Name;
                    existing.Description = p.Description;
                    existing.Price = p.Price;
                    existing.Currency = p.Currency;
                    existing.CategoryId = p.CategoryId;
                    existing.UpdatedAt = DateTime.UtcNow;

                    _db.Attributes.RemoveRange(existing.Attributes);
                    existing.Attributes = p.Attributes?
                        .Select(a => new ProductAttribute { ProductId = existing.ProductId, Name = a.Name, Value = a.Value })
                        .ToList();

                    _db.ProductImages.RemoveRange(existing.TourImages);
                    existing.TourImages = p.Images?
                        .Select(i => new ProductImage { ProductId = existing.ProductId, ImageUrl = i })
                        .ToList();

                    _db.ProductAvailabilities.RemoveRange(existing.TourAvailabilities);
                    existing.TourAvailabilities = p.Availabilities?
                        .Select(a => new ProductAvailability
                        {
                            ProductId = existing.ProductId,
                            Date = a.Date,
                            AvailableUnits = a.AvailableUnits,
                            Price = a.Price
                        })
                        .ToList();
                }

                count++;
            }

            await _db.SaveChangesAsync();
            return count;
        }


        // ---------------- MAPPERS ----------------

        private ProductResponseDto MapToDto(Product p)
        {
            return new ProductResponseDto
            {
                ProductId = p.ProductId,
                ExternalId = p.ExternalId,
                Provider = p.Provider,
                ProductType = p.ProductType,
                Name = p.Name,
                Description = p.Description,
                CategoryName = p.Category.CategoryName,
                Price = p.Price,
                Currency = p.Currency,
                Location = p.Location,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                ImageUrls = p.TourImages.Select(i => i.ImageUrl).ToList(),
                Attributes = p.Attributes.Select(a => new AttributeItem { Name = a.Name, Value = a.Value }).ToList(),
                Availabilities = p.TourAvailabilities.Select(a => new AvailabilityItem
                {
                    Date = a.Date,
                    AvailableUnits = a.AvailableUnits,
                    Price = a.Price
                }).ToList()
            };
        }

        private Product MapToEntity(ProductRequestDto dto)
        {
            return new Product
            {
                ExternalId = dto.ExternalId,
                Provider = dto.Provider,
                ProductType = dto.ProductType,
                Name = dto.Name,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                Price = dto.Price,
                Currency = dto.Currency,
                Location = dto.Location,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Attributes = dto.Attributes?.Select(a => new ProductAttribute { Name = a.Name, Value = a.Value }).ToList(),
                TourImages = dto.Images?.Select(img => new ProductImage { ImageUrl = img }).ToList(),
                TourAvailabilities = dto.Availabilities?.Select(a => new ProductAvailability
                {
                    Date = a.Date,
                    AvailableUnits = a.AvailableUnits,
                    Price = a.Price
                }).ToList()
            };
        }

        private Product MapSyncToEntity(ProductSyncDto p)
        {
            return new Product
            {
                ExternalId = p.ExternalId,
                Provider = "Bokun",
                ProductType = p.ProductType,
                Name = p.Name,
                Description = p.Description,
                CategoryId = p.CategoryId,
                Price = p.Price,
                Currency = p.Currency,
                Location = p.Location,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Attributes = p.Attributes?.Select(a => new ProductAttribute { Name = a.Name, Value = a.Value }).ToList(),
                TourImages = p.Images?.Select(i => new ProductImage { ImageUrl = i }).ToList(),
                TourAvailabilities = p.Availabilities?.Select(a => new ProductAvailability
                {
                    Date = a.Date,
                    AvailableUnits = a.AvailableUnits,
                    Price = a.Price
                }).ToList()
            };
        }
    }

}
