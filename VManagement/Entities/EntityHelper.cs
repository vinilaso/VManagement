using System.Reflection;
using VManagement.Commons.Entities;
using VManagement.Commons.Entities.Attributes;
using VManagement.Commons.Interfaces;
using VManagement.Database.Clauses;

namespace VManagement.Database.Entities
{
    public static class EntityHelper<TEntity> where TEntity : CoreEntity
    {
        public static string GetEntityName()
        {
            EntityName? nameAttribute = typeof(TEntity).GetCustomAttribute(typeof(EntityName)) as EntityName;

            if (nameAttribute == null)
                throw new OperationCanceledException($"The object {typeof(TEntity)} does not have the EntityName attribute implemented.");

            return nameAttribute.Name;
        }

        public static List<string?> GetEntityFields()
        {
            EntitySchema schema = new EntitySchema("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TABLENAME");
            Restriction restriction = new Restriction();
            restriction.Parameters.Add("@TABLENAME", GetEntityName());

            List<CoreEntity> entities = Entity.GetMany(schema, restriction);
            return entities.Select(e => e.Fields["COLUMN_NAME"]?.ToString()).ToList();
        }

        public static List<string?> GetRequiredFields()
        {
            EntitySchema schema = new EntitySchema("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TABLENAME AND IS_NULLABLE = @NULLABLE AND COLUMN_NAME <> 'ID'");
            Restriction restriction = new Restriction();
            restriction.Parameters.Add("@TABLENAME", GetEntityName());
            restriction.Parameters.Add("@NULLABLE", "NO");

            List<CoreEntity> entities = Entity.GetMany(schema, restriction);
            return entities.Select(e => e.Fields["COLUMN_NAME"]?.ToString()).ToList();
        }

        public static IFieldValue CreateFieldValue(string name, object? value)
        {
            return new FieldValue(name, value);
        }
    }
}
