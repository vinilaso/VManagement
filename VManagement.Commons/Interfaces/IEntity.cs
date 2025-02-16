namespace VManagement.Commons.Interfaces
{
    public interface IEntity
    {
        string TableName { get; }
        IFieldCollection Fields { get; }
        long Id { get; set; }
        IEnumerable<string> AllFieldNames(bool ignoreId = false);
        void ValidateRequiredFields();
    }
}
