using Microsoft.Data.SqlClient;
using VManagement.Commons.Entities;
using VManagement.Database.Clauses;
using VManagement.Database.Commands;
using VManagement.Database.Connection;
using VManagement.Database.Utils;

namespace VManagement.Database.Entities
{
    public class Entity
    {
        public static void Execute(RawCommand rawCommand)
        {
            using VManagementConnection connection = new VManagementConnection();
            var command = connection.CreateCommand();
            command.CommandText = rawCommand.CommandText;
            command.SetParameters(rawCommand.Parameters);

            command.ExecuteNonQuery();
        }

        public static CoreEntity? GetFirstOrDefault(EntitySchema schema)
        {
            return GetFirstOrDefault(schema, Restriction.Empty);
        }

        public static CoreEntity? GetFirstOrDefault(EntitySchema schema, Restriction restriction)
        {
            using VManagementConnection connection = new VManagementConnection();
            var command = connection.CreateCommand();
            command.CommandText = CommandBuilder.FormatQuery(schema.SqlQuery, restriction);
            command.SetParameters(restriction.Parameters);

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                CoreEntity result = new CoreEntity();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    result.Fields.Add(reader.GetName(i), reader.GetNullableValue(i));
                }

                return result;
            }

            return default;
        }

        public static List<CoreEntity> GetAll(EntitySchema schema)
        {
            return GetMany(schema, Restriction.Empty);
        }

        public static List<CoreEntity> GetMany(EntitySchema schema, Restriction restriction)
        {
            using VManagementConnection connection = new VManagementConnection();
            var command = connection.CreateCommand();
            command.CommandText = CommandBuilder.FormatQuery(schema.SqlQuery, restriction);
            command.SetParameters(restriction.Parameters);

            using SqlDataReader reader = command.ExecuteReader();

            List<CoreEntity> result = new List<CoreEntity>();
            while (reader.Read())
            {
                CoreEntity entity = new CoreEntity();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    entity.Fields.Add(reader.GetName(i), reader.GetNullableValue(i));
                }

                result.Add(entity);
            }
            return result;
        }
    }
}
