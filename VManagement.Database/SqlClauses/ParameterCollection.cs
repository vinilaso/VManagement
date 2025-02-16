using System.Collections;

namespace VManagement.Database.SqlClauses
{
    public class ParameterCollection : ICollection<Parameter>
    {
        public ICollection<Parameter> Items { get; set; } = new List<Parameter>();

        public int Count => Items.Count;

        public bool IsReadOnly => Items.IsReadOnly;

        public void Add(Parameter item)
        {
            Items.Add(item);
        }

        public void Add(string name, object? value)
        {
            Items.Add(new Parameter(name, value));
        }

        public void Clear()
        {
            Items.Clear();
        }

        public bool Contains(Parameter item)
        {
            return Items.Contains(item);
        }

        public bool Contains(string name)
        {
            return Items.Any(i => i.Name == name);
        }

        public void CopyTo(Parameter[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Parameter> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        public bool Remove(Parameter item)
        {
            return Items.Remove(item);
        }

        public bool Remove(string name)
        {
            var item = Items.FirstOrDefault(i => i.Name == name);
            if (item != null)
                return Items.Remove(item);
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
