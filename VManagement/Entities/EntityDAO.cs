using Microsoft.Data.SqlClient;
using VManagement.Commons.Entities;
using VManagement.Database.Clauses;
using VManagement.Database.Commands;
using VManagement.Database.Connection;
using VManagement.Database.Utils;

namespace VManagement.Database.Entities
{
    public abstract class EntityDAO<TEntity> where TEntity : CoreEntity, new()
    {
        public static void Save(TEntity entity)
        {
            using VManagementConnection connection = new VManagementConnection();
            var command = connection.CreateCommand();

            CommandBuilder commandBuilder = new CommandBuilder(entity);
            command.CommandText = commandBuilder.BuildInsertClause();
            command.SetParameters(entity);

            entity.Id = command.ExecuteScalar<long>();
        }

        public static void Update(TEntity entity)
        {
            using VManagementConnection connection = new VManagementConnection();
            var command = connection.CreateCommand();

            CommandBuilder commandBuilder = new CommandBuilder(entity, Restriction.FromId(entity.Id, false));
            command.CommandText = commandBuilder.BuildUpdateClause();
            command.SetParameters(entity);
            command.SetParameters(Restriction.FromId(entity.Id, false).Parameters);

            command.ExecuteNonQuery();
        }

        public static void Delete(TEntity entity)
        {
            using VManagementConnection connection = new VManagementConnection();
            var command = connection.CreateCommand();

            CommandBuilder commandBuilder = new CommandBuilder(entity, Restriction.FromId(entity.Id, false));
            command.CommandText = commandBuilder.BuildDeleteClause();
            command.SetParameters(Restriction.FromId(entity.Id).Parameters);

            command.ExecuteNonQuery();
        }

        public static TEntity Get(long id)
        {
            return Get(Restriction.FromId(id));
        }

        public static TEntity Get(Restriction restriction)
        {
            CoreEntity model = CreateTypeInstance();

            using VManagementConnection connection = new VManagementConnection();
            var command = connection.CreateCommand();

            CommandBuilder commandBuilder = new CommandBuilder(model, restriction);
            command.CommandText = commandBuilder.BuildSelectClause();
            command.SetParameters(restriction.Parameters);

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                TEntity entity = TEntityFromDataRader(reader);
                entity.Schema.EntityName = model.Schema.EntityName;
                return entity;
            }

            throw new OperationCanceledException($"No records found with this restriction. ({restriction})");
        }

        public static TEntity? GetFirstOrDefault(long id)
        {
            return GetFirstOrDefault(Restriction.FromId(id));
        }

        public static TEntity? GetFirstOrDefault(Restriction restriction)
        {
            try
            {
                return Get(restriction);
            }
            catch (OperationCanceledException)
            {
                return default;
            }
            catch
            {
                throw;
            }
        }

        public static List<TEntity> GetAll()
        {
            return GetMany(Restriction.Empty);
        }

        public static List<TEntity> GetMany(Restriction restriction)
        {
            CoreEntity model = CreateTypeInstance();

            using VManagementConnection connection = new VManagementConnection();
            var command = connection.CreateCommand();

            CommandBuilder commandBuilder = new CommandBuilder(model, restriction);
            command.CommandText = commandBuilder.BuildSelectClause();
            command.SetParameters(restriction.Parameters);

            using SqlDataReader reader = command.ExecuteReader();

            List<TEntity> entities = new List<TEntity>();

            while (reader.Read())
            {
                TEntity entity = TEntityFromDataRader(reader);
                entity.Schema.EntityName = model.Schema.EntityName;
                entities.Add(entity);
            }

            return entities;
        }

        public static IEnumerable<TEntity> FetchMany(Restriction restriction)
        {
            CoreEntity model = CreateTypeInstance();

            using VManagementConnection connection = new VManagementConnection();
            var command = connection.CreateCommand();

            CommandBuilder commandBuilder = new CommandBuilder(model, restriction);
            command.CommandText = commandBuilder.BuildSelectClause();
            command.SetParameters(restriction.Parameters);

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                TEntity entity = TEntityFromDataRader(reader);
                entity.Schema.EntityName = model.Schema.EntityName;

                yield return entity;
            }
        }

        public static bool Exists(Restriction restriction)
        {
            CoreEntity model = CreateTypeInstance();

            using VManagementConnection connection = new VManagementConnection();
            var command = connection.CreateCommand();

            CommandBuilder commandBuilder = new CommandBuilder(model, restriction);
            command.CommandText = commandBuilder.BuildExistsClause();
            command.SetParameters(restriction.Parameters);

            return command.ExecuteScalar<int>() == 1;
        }

        private static TEntity TEntityFromDataRader(SqlDataReader reader)
        {
            TEntity entity = new TEntity();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string fieldName = reader.GetName(i);
                object? value = reader.GetNullableValue(i);

                if (entity.Fields.Any(f => f.Name == fieldName))
                {
                    entity.Fields[fieldName] = value;
                }
                else
                {
                    entity.Fields.Add(fieldName, value);
                }

                entity.Values.Add(new FieldValue(fieldName, value));
            }

            return entity;
        }

        private static CoreEntity CreateTypeInstance()
        {
            var entity = new CoreEntity();
            entity.Schema.EntityName = EntityHelper<TEntity>.GetEntityName();

            foreach (var field in EntityHelper<TEntity>.GetEntityFields())
            {
                if (field == null)
                    continue;

                entity.Fields.Add(field, null);
                entity.Values.Add(new FieldValue(field, null));
            }

            return entity;
        }
    }
}
