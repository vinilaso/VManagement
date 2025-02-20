using VManagement.Commons.Enum;
using VManagement.Commons.Interfaces;

namespace VManagement.Core.Entities
{
    public abstract class BusinessEntity<T> : EntityController<T>, IEntity where T : IEntity, new()
    {
        public IField this[string name]
        {
            get
            {
                var field = Fields.FirstOrDefault(field => field.Name == name);

                if (field == null)
                    throw new NotImplementedException($"There is no field named '{name}'");

                return field;
            }
            set
            {
                var field = Fields.FirstOrDefault(field => field.Name == name);

                if (field == null)
                    throw new NotImplementedException($"There is no field named '{name}'");

                field = value;
            }
        }

        public EntityState State { get; set; }

        public IFieldCollection Fields { get; } = new FieldAggregation();

        public IFieldValueCollection FieldValues { get; }

        public virtual string TableName => throw new NotImplementedException();

        public virtual long Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public virtual IEnumerable<string> AllFieldNames(bool ignoreId = false)
        {
            throw new NotImplementedException();
        }

        public virtual void ValidateRequiredFields()
        {
            throw new NotImplementedException();
        }
    }
}
