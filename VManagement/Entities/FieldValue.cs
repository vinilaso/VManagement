using VManagement.Commons.Interfaces;

namespace VManagement.Database.Entities
{
    internal class FieldValue : IFieldValue
    {
        private string _name = string.Empty;
        private object? _value;
        private object? _originalValue;

        public FieldValue(string name)
        {
            _name = name;
        }

        public FieldValue(string name, object? value)
        {
            SetName(name);
            SetValue(value);
        }

        public string Name => _name;

        public object? Value
        {
            get => _value;
            set => _value = value;
        }

        public object? OriginalValue => _originalValue;

        public bool IsNull => Value is null;

        public bool Changed
        {
            get
            {
                if (_value is null && _originalValue is null)
                    return false;

                if (_value is null || _originalValue is null)
                    return true;

                return !_value.Equals(_originalValue);
            }
        }

        internal void SetValue(object? value)
        {
            _value = value;
            _originalValue = value;
        }

        internal void SetName(string name)
        {
            _name = name;
        }
    }
}
