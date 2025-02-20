using VManagement.Commons.Interfaces;

namespace VManagement.Database.Entities
{
    internal class FieldValue : IFieldValue
    {
        public string Name { get; private set; } = string.Empty;
        public object? Value { get; private set; }

        public object? OriginalValue { get; private set; }

        public bool Changed => Value != OriginalValue;

        public bool IsNull => Value == null;

        internal FieldValue(string name, object? value)
        {
            Name = name;
            Value = value;
            OriginalValue = value;
        }

        #pragma warning disable CS8600
        public T? GetOriginalValueAs<T>()
        {
            try
            {
                return (T?)OriginalValue;
            }
            catch
            {
                return default;
            }
        }

        public T? GetValueAs<T>()
        {
            try
            {
                return (T?)Value;
            }
            catch
            {
                return default;
            }
        }
    }
}
