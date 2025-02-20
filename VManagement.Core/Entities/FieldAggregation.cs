using System.Collections;
using VManagement.Commons.Interfaces;

namespace VManagement.Core.Entities
{
    public class FieldAggregation : IFieldCollection
    {
        private ICollection<IField> _fields = new List<IField>();

        public int Count => _fields.Count;
        public bool IsReadOnly => _fields.IsReadOnly;
        public object? this[string name]
        {
            get
            {
                return _fields.FirstOrDefault(field => field.Name == name)?.Value;
            }
            set
            {
                IField? field = _fields.FirstOrDefault(field => field.Name == name);

                if (field == null)
                    throw new ArgumentException($"There is no field named '{name}'");

                field.Value = value;
            }
        }

        public void Add(IField item)
        {
            VerifyDuplicity(item.Name);
            _fields.Add(item);
        }

        public void Add(string name, object? value) =>
            this.Add(new Field(name, value));

        public void Clear() =>
            _fields.Clear();

        public bool Contains(IField item) =>
            _fields.Contains(item);

        public bool Contains(string name) =>
            _fields.Any(f => f.Name == name);

        public void CopyTo(IField[] array, int arrayIndex) =>
            _fields.CopyTo(array, arrayIndex);

        public IEnumerator<IField> GetEnumerator() =>
            _fields.GetEnumerator();

        public bool Remove(IField item) =>
            _fields.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() =>
            _fields.GetEnumerator();

        private void VerifyDuplicity(string name)
        {
            if (_fields.Any(f => f.Name == name))
                throw new ArgumentException($"There is already a field named '{name}'");
        }
    }
}
