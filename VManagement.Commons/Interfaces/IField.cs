namespace VManagement.Commons.Interfaces
{
    public interface IField
    {
        string Name { get; set; }
        object? Value { get; set; }

        string AsParameter();
    }
}
