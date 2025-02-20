using VManagement.Commons.Interfaces;

namespace VManagement.Core.Entities
{
    public class Field : IField
    {
        public string Name { get; set; } = string.Empty;
        public object? Value { get; set; } = null;
        public object? OriginalValue { get; private set; } = null;
        public bool Changed => Value != OriginalValue;

        internal Field() { }

        internal Field(string name, object? value)
        {
            Name = name;
            Value = value;
            OriginalValue = value;
        }

        public void SetValue(object? value)
        {
            throw new NotImplementedException("You can't use this method outside the database layer.");
        }

        public string AsParameter()
        {
            return string.Format("@{0}", Name);
        }

        public override string ToString()
        {
            return Value?.ToString() ?? string.Empty;
        }
    }
}
