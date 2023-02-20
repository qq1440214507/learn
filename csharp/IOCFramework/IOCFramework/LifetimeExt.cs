using Microsoft.Extensions.DependencyInjection;

namespace IOCFramework;

internal static class LifetimeExt
{
    public static Lifetime AsContainerLifetime(this ServiceLifetime lifetime)
    {
        return lifetime switch
        {
            ServiceLifetime.Scoped => Lifetime.Self,
            ServiceLifetime.Singleton => Lifetime.Root,
            _ => Lifetime.Transient,
        };
    }
}