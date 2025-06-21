namespace VManagement.Commons.Entities.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EntityColumnNameAttribute(string name) : Attribute
    {
        public string Name { get; set; } = name;
    }
}
