namespace VManagement.Commons.Interfaces
{
    public interface IFieldValue
    {
        string Name { get; } 
        object? Value { get; set; }
        object? OriginalValue { get; }
        bool IsNull { get; }
        bool Changed { get; }
    }
}
