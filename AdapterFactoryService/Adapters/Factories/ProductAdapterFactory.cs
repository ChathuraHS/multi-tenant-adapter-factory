using AdapterFactoryService.Adapters.Providers;

namespace AdapterFactoryService.Adapters.Factories
{
    public class ProductAdapterFactory : IProductAdapterFactory
    {
        private readonly IServiceProvider _service;
        //IServiceProvider is .NET’s built-in dependency injection container.
        //It allows you to create or retrieve objects that were registered in Program.cs.

        public ProductAdapterFactory(IServiceProvider service)
        {
            _service = service;
        }

        public IProductProviderAdapter GetAdapter(string provider)
        {
            return provider.ToLower() switch
            {
                //Gives an instance of the BokunProductProviderAdapter.
                "bokun" => _service.GetRequiredService<BokunProductProviderAdapter>(),
                _ => throw new Exception("Provider not supported")
            };
        }
    }
}
