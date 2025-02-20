namespace VManagement.Commons.Interfaces
{
    public interface IFieldValueCollection : ICollection<IFieldValue>
    {
        object? this[string name] { get; }
    }
}
