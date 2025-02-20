namespace VManagement.Commons.Interfaces
{
    public interface IFieldValue
    {
        string Name { get; }
        object? Value { get; }
        object? OriginalValue { get; }
        bool Changed { get; }
        bool IsNull { get; }

        T? GetValueAs<T>();

        T? GetOriginalValueAs<T>();
    }
}
