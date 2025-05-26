namespace VManagement.Core.Entities
{
    public abstract class ListField
    {
        public int Index { get; set; }
        public string Description { get; set; } = string.Empty;

        public ListField() { }

        public ListField(int index, string description)
        {
            Index = index;
            Description = description;
        }
    }
}
