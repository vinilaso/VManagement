using VManagement.Commons.Enum;
using VManagement.Commons.Interfaces;
using VManagement.Database.Entities;
using VManagement.Database.SqlClauses;

namespace VManagement.Core.Entities
{
    public class EntityController<T> where T : IEntity, new()
    {
        public static string EntityName => new T().TableName;

        protected EntityDAO<T>? _dao;

        public static T Create()
        {
            T instance = new T()
            {
                State = EntityState.Created
            };

            foreach (var field in instance.AllFieldNames())
                instance.Fields.Add(field, null);

            return instance;
        }

        public static T? GetFirstOrDefault(long id)
        {
            return GetFirstOrDefault(Restriction.FromId(id));
        }

        public static T? GetFirstOrDefault(Restriction restriction)
        {
            T? entity = EntityDAO<T>.GetFirstOrDefault(restriction);

            if (entity != null)
                entity.State = EntityState.Loaded;

            return entity;
        }

        public virtual void Edit()
        {
            _dao!.Entity.State = EntityState.Editing;
        }

        public virtual void Delete()
        {

        }

        protected virtual void Deleting()
        {
            if (_dao!.Entity.State == EntityState.Created)
            {
                throw new InvalidOperationException("You can't delete an entity that isn't saved...");
            }

            if (_dao.Entity.State == EntityState.Editing)
            {
                throw new InvalidOperationException("You can't delete an entity that is being edited...");
            }

            _dao.Delete();
        }

        protected virtual void Deleted()
        {
            _dao!.Entity.State = EntityState.Deleted;
        }

        public virtual void Save()
        {
            if (_dao == null)
                throw new ArgumentException("The _dao instance can't be null...");

            _dao.Entity.ValidateRequiredFields();
            Saving();
        }

        protected virtual void Saving()
        {

            if (_dao!.Entity.State == EntityState.Editing)
            {
                _dao.Update();
            }

            else if (_dao.Entity.State == EntityState.Created)
            {
                _dao!.Save();
            }

            else if (_dao.Entity.State == EntityState.Deleted)
            {
                throw new InvalidOperationException("This entity was deleted.");
            }

            else
            {
                throw new InvalidOperationException("The entity isn't in edit mode.");
            }
            
            Saved();
        }

        protected virtual void Saved()
        {
            _dao!.Entity.State = EntityState.Loaded;
        }
    }
}
