using VManagement.Commons.Interfaces;

namespace VManagement.Commons.Entities
{
    public class FieldValueCollection : List<IFieldValue>
    {
        public IFieldValue this[string fieldName]
        {
            get
            {
                IFieldValue? fieldValue = Find(fdVal => fdVal.Name == fieldName);

                if (fieldValue == null)
                    throw new ArgumentException($"There are no fields named {fieldName} in the collection.");

                return fieldValue;
            }
        }
    }
}
