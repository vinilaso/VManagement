namespace VManagement.Commons.Entities.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityName(string name) : Attribute
    {
        public string Name { get; set; } = name;
    }
}
