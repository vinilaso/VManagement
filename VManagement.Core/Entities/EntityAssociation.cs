using VManagement.Commons.Entities;
using VManagement.Commons.Utility;
using VManagement.Core.Business;
using VManagement.Database.Entities;

namespace VManagement.Core.Entities
{
    public class EntityAssociation<TEntity> where TEntity : BusinessEntity, new()
    {
        private long? _id = null;
        private CoreEntity? _ownerEntity;
        private TEntity? _instance = null;

        public string FieldName { get; set; } = string.Empty;
        public long? Id
        {
            get => _id;
            set
            {
                _id = value;

                if (_ownerEntity != null)
                    _ownerEntity.Fields[FieldName] = value;
            }
        }
        public TEntity? Instance
        {
            get
            {
                if (_id == null)
                    return null;

                if (_instance == null)
                    LoadInstance();

                return _instance;
            }
            set
            {
                if (value == null)
                {
                    _id = null;
                }
                else
                {
                    _id = value.Id;
                    LoadInstance();
                }

                if (_ownerEntity != null)
                {
                    _ownerEntity.Fields[FieldName] = _id;
                }
            }
        }

        public EntityAssociation() { }

        public EntityAssociation(CoreEntity entity, string fieldName)
        {
            _ownerEntity = entity;
            _id = _ownerEntity.Fields[fieldName].SafeToInt64();

            FieldName    = fieldName;
        }

        private void LoadInstance()
        {
            _instance = EntityDAO<TEntity>.Get(_id ?? 0);
        }
    }
}
