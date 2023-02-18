using System.Collections.Concurrent;

namespace IOCFramework;

public class Container:IServiceProvider,IDisposable
{
    internal readonly Container _root;
    internal readonly ConcurrentDictionary<Type, ServiceRegistry> _registries;
    private readonly ConcurrentDictionary<Key, object?> _services;
    private readonly ConcurrentBag<IDisposable> _disposables;
    private volatile bool _disposed;

    public Container()
    {
        _root = this;
        _registries = new ConcurrentDictionary<Type, ServiceRegistry>();
        _services = new ConcurrentDictionary<Key, object?>();
        _disposables = new ConcurrentBag<IDisposable>();
    }

    internal Container(Container parent)
    {
        _root = parent._root;
        _registries = _root._registries;
        _services = _root._services;
        _disposables = _root._disposables;

    }

    private void EnsureNotDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException("Container");
        }
    }

    public Container Register(ServiceRegistry registry)
    {
        EnsureNotDisposed();
        if (_registries.TryGetValue(registry.ServiceType,out var existing))
        {
            _registries[registry.ServiceType] = registry;
            registry.Next = existing;
        }
        else
        {
            _registries[registry.ServiceType] = registry;
        }

        return this;
    }

    public object? GetService(Type serviceType)
    {
        EnsureNotDisposed();
        if (serviceType == typeof(Container) || serviceType == typeof(IServiceProvider))
        {
            return this;
        }

        ServiceRegistry? registry;
        if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            var elementType = serviceType.GetGenericArguments()[0];
            if (!_registries.TryGetValue(elementType,out registry))
            {
                return Array.CreateInstance(elementType, 0);
            }

            var registries = registry.AsEnumerable();
            var services = registries.Select(it => GetServiceCore(it, Type.EmptyTypes)).Reverse().ToArray();
            var array = Array.CreateInstance(elementType, services.Length);
            services.CopyTo(array,0);
            return array;
        }

        if (serviceType.IsGenericType && !_registries.ContainsKey(serviceType))
        {
            var definition = serviceType.GetGenericTypeDefinition();
            return _registries.TryGetValue(definition,out registry) ? GetServiceCore(registry,serviceType.GetGenericArguments()) : null;
        }

        return _registries.TryGetValue(serviceType, out registry) ? GetServiceCore(registry, Type.EmptyTypes) : null;
    }

    private object? GetServiceCore(ServiceRegistry registry, Type[] genericArguments)
    {
        var key = new Key(registry, genericArguments);
        switch (registry.Lifetime)
        {
            case Lifetime.Root:
                return GetOrCreate(_root._services, _root._disposables);
            case Lifetime.Self:
                return GetOrCreate(_services, _disposables);
            case Lifetime.Transient:
            default:
            {
                var service = registry.Factory(this, genericArguments);
                if (service is IDisposable disposable && !disposable.Equals(this))
                {
                    _disposables.Add(disposable);
                }

                return service;
            }
        }

        object? GetOrCreate(ConcurrentDictionary<Key, object?> services, ConcurrentBag<IDisposable> disposables)
        {
            if (services.TryGetValue(key,out var service))
            {
                return service;
            }

            service = registry.Factory(this, genericArguments);
            services[key] = service;
            if (service is IDisposable disposable)
            {
                disposables.Add(disposable);
            }
            return service;
        }

    }

    public void Dispose()
    {
        _disposed = true;
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
        _disposables.Clear();
        _services.Clear();
    }
}