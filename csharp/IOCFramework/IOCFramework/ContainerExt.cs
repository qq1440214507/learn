using System.Dynamic;
using System.Reflection;

namespace IOCFramework;

public static class ContainerExtensions
{
       public static Container Register(this Container container, Type from, Type to, Lifetime lifetime)
        {
            Func<Container, Type[], object?> factory = (c, arguments) => Create(c, to, arguments);
            container.Register(new ServiceRegistry(from, lifetime, factory));
            return container;
        }

        public static Container Register<TFrom, TTo>(this Container container, Lifetime lifetime) where TTo : TFrom
            => container.Register(typeof(TFrom), typeof(TTo), lifetime);

        public static Container Register(this Container container, Type serviceType, object instance)
        {
            Func<Container, Type[], object?> factory = (_, arguments) => instance;
            container.Register(new ServiceRegistry(serviceType, Lifetime.Root, factory));
            return container;
        }

        public static Container Register<TService>(this Container container, TService instance) where TService : class
        {
            Func<Container, Type[], object?> factory = (_, arguments) => instance;
            container.Register(new ServiceRegistry(typeof(TService), Lifetime.Root, factory));
            return container;
        }

        public static Container Register(this Container container, Type serviceType,
            Func<Container, object> factory, Lifetime lifetime)
        {
            container.Register(new ServiceRegistry(serviceType, lifetime, (_, arguments) => factory(_)));
            return container;
        }

        public static Container Register<TService>(this Container container, Func<Container, TService> factory, Lifetime lifetime) where TService : class
        {
            container.Register(new ServiceRegistry(typeof(TService), lifetime, (c, arguments) => factory(c)));
            return container;
        }

        public static Container Register(this Container container, Assembly assembly)
        {
            var typedAttributes = from type in assembly.GetExportedTypes()
                                  let attribute = type.GetCustomAttribute<MapToAttribute>()
                                  where attribute != null
                                  select new { ServiceType = type, Attribute = attribute };
            foreach (var typedAttribute in typedAttributes)
            {
                container.Register(typedAttribute.Attribute.ServiceType, typedAttribute.ServiceType, typedAttribute.Attribute.Lifetime);
            }
            return container;
        }

        public static T? GetService<T>(this Container container) where T : class
            => (T?)container.GetService(typeof(T));
        public static IEnumerable<T> GetServices<T>(this Container container) 
            => container.GetService<IEnumerable<T>>()??Array.Empty<T>();
        public static Container CreateChild(this Container container) => new(container);


        private static object? Create(Container container, Type type, Type[] genericArguments)
        {
            if (genericArguments.Length > 0)
            {
                type = type.MakeGenericType(genericArguments);
            }
            var constructors = type.GetConstructors();
            if (constructors.Length == 0)
            {
                throw new InvalidOperationException($"Cannot create the instance of {type} which does not have a public constructor.");
            }
            var constructor = constructors.FirstOrDefault(it =>
                it.GetCustomAttributes(false).OfType<InjectionAttribute>().Any());
            constructor ??= constructors.Last();
            var parameters = constructor.GetParameters();
            if (parameters.Length == 0)
            {
                return Activator.CreateInstance(type);
            }
            var arguments = new object?[parameters.Length];
            for (int index = 0; index < arguments.Length; index++)
            {
                arguments[index] = container.GetService(parameters[index].ParameterType);
            }
            return constructor.Invoke(arguments);
        }
}