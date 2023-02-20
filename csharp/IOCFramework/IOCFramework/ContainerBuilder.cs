using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace IOCFramework;

public class ContainerBuilder
{
    private readonly Container _container;

    public ContainerBuilder(Container container)
    {
        _container = container;
        _container.Register<IServiceScopeFactory>(c => new ServiceScopeFactory(c.CreateChild()),Lifetime.Transient);
    }

    public IServiceProvider BuildServiceProvider() => _container;

    public ContainerBuilder Register(Assembly assembly)
    {
        _container.Register(assembly);
        return this;
    }

    private class ServiceScope : IServiceScope
    {
        public IServiceProvider ServiceProvider { get;}

        public ServiceScope(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void Dispose()
        {
            (ServiceProvider as IDisposable)?.Dispose();
        }
    }

    private class ServiceScopeFactory : IServiceScopeFactory
    {
        private readonly Container _container;

        public ServiceScopeFactory(Container container)
        {
            _container = container;
        }

        public IServiceScope CreateScope()
        {
            return new ServiceScope(_container);
        }
    }
}