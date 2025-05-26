using VManagement.Commons.Interfaces;
using VManagement.Commons.Utility;

namespace VManagement.Commons.Entities
{
    public class CoreEntity
    {
        public EntitySchema Schema { get; } = new EntitySchema();
        public FieldCollection Fields { get; }
        public FieldValueCollection Values { get; } = new FieldValueCollection();

        public CoreEntity()
        {
            Fields = new FieldCollection(this);
        }

        public long? Id
        {
            get => Fields["ID"].ToInt64();
            set => Fields["ID"] = value;
        }
    }
}
