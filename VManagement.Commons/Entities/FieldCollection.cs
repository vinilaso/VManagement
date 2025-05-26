namespace VManagement.Commons.Entities
{
    public class FieldCollection : List<EntityField>
    {
        private CoreEntity _owner;

        public FieldCollection(CoreEntity owner)
        {
            _owner = owner;
        }

        public object? this[string fieldName]
        {
            get
            {
                return ByName(fieldName).Value;
            }
            set
            {
                ByName(fieldName).Value = value;
                _owner.Values[fieldName].Value = value;
            }
        }

        public void Add(string fieldName, object? value)
        {
            Add(new EntityField(fieldName, value));
        }

        private EntityField ByName(string fieldName)
        {
            EntityField? aField = Find(field => field.Name == fieldName);

            if (aField == null)
                throw new KeyNotFoundException($"There are no fields named {fieldName} in the collection.");

            return aField;
        }
    }
}
