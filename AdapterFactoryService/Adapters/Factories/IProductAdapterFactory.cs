using AdapterFactoryService.Adapters.Providers;

namespace AdapterFactoryService.Adapters.Factories
{
    public interface IProductAdapterFactory
    {
        IProductProviderAdapter GetAdapter(string provider);
    }
}
