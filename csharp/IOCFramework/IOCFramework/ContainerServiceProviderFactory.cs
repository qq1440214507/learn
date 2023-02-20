using Microsoft.Extensions.DependencyInjection;

namespace IOCFramework;

public class ContainerServiceProviderFactory:IServiceProviderFactory<ContainerBuilder>
{
    public ContainerBuilder CreateBuilder(IServiceCollection services)
    {
        var container = new Container();
        foreach (var service in services)
        {
            if (service.ImplementationFactory != null)
            {
                container.Register(service.ServiceType, provider => service.ImplementationFactory(provider),
                    service.Lifetime.AsContainerLifetime());
            }
            else if (service.ImplementationInstance != null)
            {
                container.Register(service.ServiceType, service.ImplementationInstance);
            }
            else
            {
                container.Register(service.ServiceType,
                    service.ImplementationType ?? throw new InvalidOperationException("error register"),
                    service.Lifetime.AsContainerLifetime());
            }
        }

        return new ContainerBuilder(container);
    }

    public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
    {
        return containerBuilder.BuildServiceProvider();
    }
}