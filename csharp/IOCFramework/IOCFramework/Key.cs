namespace IOCFramework;

internal class Key:IEquatable<Key>
{
    public ServiceRegistry Registry { get; }
    public Type[] GenericArguments { get; }

    public Key(ServiceRegistry registry, Type[] genericArguments)
    {
        Registry = registry;
        GenericArguments = genericArguments;
    }

    public override bool Equals(object? obj)
    {
        return obj is Key key && Equals(key);
    }

    public override int GetHashCode()
    {
        var hashCode = Registry.GetHashCode();
        return GenericArguments.Aggregate(hashCode, (current, t) => current ^ t.GetHashCode());
    }

    public bool Equals(Key? other)
    {
        if (Registry != other?.Registry)
        {
            return false;
        }
        if (GenericArguments.Length != other.GenericArguments.Length)
        {
            return false;
        }

        return !GenericArguments.Where((t, index) => t != other.GenericArguments[index]).Any();
    }
}