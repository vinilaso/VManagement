namespace VManagement.Commons.Interfaces
{
    public interface IField
    {
        string Name { get; set; }
        object? Value { get; set; }
        object? OriginalValue { get; }
        bool Changed { get; }

        void SetValue(object? value);
        string AsParameter();
    }
}
