namespace VManagement.Commons.Entities.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityNameAttribute(string name) : Attribute
    {
        public string Name { get; set; } = name;
    }
}
