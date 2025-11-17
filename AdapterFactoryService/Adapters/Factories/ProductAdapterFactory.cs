using AdapterFactoryService.Adapters.Providers;

namespace AdapterFactoryService.Adapters.Factories
{
    public class ProductAdapterFactory : IProductAdapterFactory
    {
        private readonly IServiceProvider _service;

        public ProductAdapterFactory(IServiceProvider service)
        {
            _service = service;
        }

        public IProductProviderAdapter GetAdapter(string provider)
        {
            return provider.ToLower() switch
            {
                "bokun" => _service.GetRequiredService<BokunProductProviderAdapter>(),
                _ => throw new Exception("Provider not supported")
            };
        }
    }
}
