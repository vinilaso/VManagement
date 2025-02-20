using VManagement.Commons.Enum;

namespace VManagement.Commons.Interfaces
{
    public interface IEntity
    {
        string TableName { get; }
        EntityState State { get; set; }
        IFieldCollection Fields { get; }
        IFieldValueCollection FieldValues { get; }
        IField this[string name] { get; set; }
        long Id { get; set; }
        IEnumerable<string> AllFieldNames(bool ignoreId = false);
        void ValidateRequiredFields();
    }
}
