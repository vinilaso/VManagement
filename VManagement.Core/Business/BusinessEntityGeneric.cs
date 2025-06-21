using System.Linq.Expressions;
using VManagement.Database.Clauses;
using VManagement.Database.Entities;
using VManagement.Database.Expressions;

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

        public static TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Get(SqlServerExpressionVisitor.ParseToRestriction(predicate));
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

        public static TEntity? GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetFirstOrDefault(SqlServerExpressionVisitor.ParseToRestriction(predicate));
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

        public static List<TEntity> GetAll()
        {
            return EntityDAO<TEntity>.GetAll();
        }

        public static List<TEntity> GetMany(Expression<Func<TEntity, bool>> predicate)
        {
            return GetMany(SqlServerExpressionVisitor.ParseToRestriction(predicate));
        }

        public static List<TEntity> GetMany(Restriction restriction)
        {
            return EntityDAO<TEntity>.GetMany(restriction);
        }

        public static bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return Exists(SqlServerExpressionVisitor.ParseToRestriction(predicate));
        }

        public static bool Exists(Restriction restriction)
        {
            return EntityDAO<TEntity>.Exists(restriction);
        }

        public static IEnumerable<TEntity> FetchMany(Expression<Func<TEntity, bool>> predicate)
        {
            Restriction restriction = SqlServerExpressionVisitor.ParseToRestriction(predicate);

            foreach (TEntity entity in EntityDAO<TEntity>.FetchMany(restriction))
            {
                yield return entity;
            }
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
