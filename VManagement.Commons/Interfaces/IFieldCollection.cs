namespace VManagement.Commons.Interfaces
{
    public interface IFieldCollection : ICollection<IField>
    {
        object? this[string name] { get; set; }
        void Add(string name, object? value);
    }
}
