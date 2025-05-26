using VManagement.Commons.Entities;
using VManagement.Commons.Enum;
using VManagement.Commons.Interfaces;
using VManagement.Database.Entities;

namespace VManagement.Core.Business
{
    public class BusinessEntity : CoreEntity
    {
        public IFieldValue this[string fieldName]
        {
            get
            {
                return Values[fieldName];
            }
        }

        public EntityState State { get; set; } = EntityState.New;
        public bool IsCreating => State == EntityState.New;
        public bool IsEditing => State == EntityState.Editing;
        public bool IsLoaded => State == EntityState.Loaded;

        public virtual void Get()
        {
            Getting();
        }

        public virtual void Save()
        {
            ValidateRequiredFields();
            Saving();
        }

        public virtual void Edit()
        {
            State = EntityState.Editing;
        }

        public virtual void Delete()
        {
            Deleting();
        }


        protected virtual void Saving()
        {
            switch (State)
            {
                case EntityState.New:
                    {
                        EntityDAO<BusinessEntity>.Save(this);
                        break;
                    }
                case EntityState.Editing:
                    {
                        HandleEditing();
                        break;
                    }
                default:
                    throw new OperationCanceledException("The entity was not in edit mode.");
            }

            Saved();
        }

        protected virtual void Saved()
        {
            State = EntityState.Loaded;
        }

        protected virtual void Editing()
        {

        }

        protected virtual void Edited()
        {

        }

        protected virtual void Getting()
        {
            Got();
        }

        protected virtual void Got()
        {
            State = EntityState.Loaded;
        }

        protected virtual void Deleting()
        {
            if (State == EntityState.New)
                throw new OperationCanceledException("You can't delete a new entity.");

            EntityDAO<BusinessEntity>.Delete(this);
            Deleted();
        }

        protected virtual void Deleted()
        {
            State = EntityState.Deleted;
        }

        protected virtual void ValidateRequiredFields()
        {
            throw new NotImplementedException();
        }

        private void HandleEditing()
        {
            Editing();
            EntityDAO<BusinessEntity>.Update(this);
            Edited();
        }
    }
}
