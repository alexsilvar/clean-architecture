namespace SharedKernel;

public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        { return false; }

        var other = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(1, (current, obj) =>
            {
                unchecked
                {
                    return current * 23 + (obj?.GetHashCode() ?? 0);
                }
            });
    }

#pragma warning disable S3875 // "operator==" should not be overloaded on reference types
    public static bool operator ==(ValueObject? a, ValueObject? b)
#pragma warning restore S3875 // "operator==" should not be overloaded on reference types
    {
        if (a is null && b is null)
        { return true; }

        if (a is null || b is null)
        { return false; }

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject? a, ValueObject? b) => !(a == b);
}
