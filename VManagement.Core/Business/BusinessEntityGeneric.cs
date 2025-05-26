using VManagement.Database.Clauses;
using VManagement.Database.Entities;

namespace VManagement.Core.Business
{
    public class BusinessEntity<TEntity> : BusinessEntity where TEntity : BusinessEntity, new()
    {
        public static TEntity CreateInstance()
        {
            TEntity entity = new TEntity();
            entity.Schema.EntityName = EntityHelper<TEntity>.GetEntityName();

            foreach (string? fieldName in EntityHelper<TEntity>.GetEntityFields())
            {
                if (fieldName == null)
                    continue;

                entity.Fields.Add(fieldName, null);
                entity.Values.Add(EntityHelper<TEntity>.CreateFieldValue(fieldName, null));
            }

            return entity;
        }

        public static TEntity Get(long id)
        {
            return Get(Restriction.FromId(id));
        }

        public static TEntity Get(Restriction restriction)
        {
            TEntity entity = EntityDAO<TEntity>.Get(restriction);
            entity.Get();
            return entity;
        }

        public static TEntity? GetFirstOrDefault(long id)
        {
            return GetFirstOrDefault(Restriction.FromId(id));
        }

        public static TEntity? GetFirstOrDefault(Restriction restriction)
        {
            TEntity? entity = EntityDAO<TEntity>.GetFirstOrDefault(restriction);

            if (entity == null)
            {
                return null;
            }

            entity.Get();
            return entity;
        }

        public List<TEntity> GetAll()
        {
            return EntityDAO<TEntity>.GetAll();
        }

        public List<TEntity> GetMany(Restriction restriction)
        {
            return EntityDAO<TEntity>.GetMany(restriction);
        }

        public static bool Exists(Restriction restriction)
        {
            return EntityDAO<TEntity>.Exists(restriction);
        }

        public static IEnumerable<TEntity> FetchMany(Restriction restriction)
        {
            foreach (TEntity entity in EntityDAO<TEntity>.FetchMany(restriction))
            {
                yield return entity;
            }
        }

        protected override void ValidateRequiredFields()
        {
            foreach (string? fieldName in EntityHelper<TEntity>.GetRequiredFields())
            {
                if (fieldName == null)
                    continue;

                if (Fields[fieldName] == null)
                    throw new ArgumentNullException($"The field {fieldName} is not nullable.");
            }
        }
    }
}
