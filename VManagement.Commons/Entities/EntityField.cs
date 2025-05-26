namespace VManagement.Commons.Entities
{
    public class EntityField
    {
        public string Name { get; set; } = string.Empty;
        public object? Value { get; set; }
        public bool Required { get; set; }

        public EntityField(string name, object? value)
        {
            Name = name;
            Value = value;
        }

        public string WithAlias(string alias)
        {
            return $"{alias}.{Name}";
        }
    }
}
