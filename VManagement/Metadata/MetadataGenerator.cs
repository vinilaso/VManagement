using System.Runtime.CompilerServices;
using System.Text.Json;
using VManagement.Commons.Entities;
using VManagement.Commons.Utility;
using VManagement.Database.Clauses;
using VManagement.Database.Entities;

[assembly: InternalsVisibleTo("VManagement.Console")]
namespace VManagement.Database.Metadata
{
    internal sealed class MetadataGenerator
    {
        internal static string ConfigurationFilePath => Path.Combine(Security.InternalFilesPath, "entityMetadata.json");

        internal static void Run()
        {
            CreateConfigurationFile();

            List<EntityMetadata> systemMetadata = SearchMetadata();
            
            PersistMetadata(systemMetadata);
        }

        private static void PersistMetadata(List<EntityMetadata> systemMetadata)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            File.WriteAllText(ConfigurationFilePath, JsonSerializer.Serialize(systemMetadata, options));
        }

        private static List<EntityMetadata> SearchMetadata()
        {
            List<EntityMetadata> result = [];

            EntitySchema tableSchema = new("SELECT DISTINCT TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS");

            foreach (CoreEntity tableEntity in Entity.GetMany(tableSchema, Restriction.Empty))
            {
                EntityMetadata metadata = new()
                {
                    EntityName = tableEntity.Fields["TABLE_NAME"].SafeToString()
                };

                EntitySchema columnSchema = new("SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TABLE_NAME");

                Restriction restriction = new();
                restriction.Parameters.Add("@TABLE_NAME", metadata.EntityName);

                foreach (CoreEntity columnEntity in Entity.GetMany(columnSchema, restriction))
                {
                    EntityColumnMetadata columnMetadata = new()
                    {
                        ColumnName = columnEntity.Fields["COLUMN_NAME"].SafeToString(),
                        ColumnType = columnEntity.Fields["DATA_TYPE"].SafeToString(),
                        DotNetPropertyType = ConvertToDotNetType(columnEntity.Fields["DATA_TYPE"].SafeToString()),
                        DotNetPropertyName = columnEntity.Fields["COLUMN_NAME"].SafeToString().Capitalize(),
                        ConversionMethod = GetConversionMethod(columnEntity.Fields["DATA_TYPE"].SafeToString())
                    };

                    metadata.Columns.Add(columnMetadata);
                }

                result.Add(metadata);
            }

            return result;
        }

        private static string ConvertToDotNetType(string dbTypeName)
        {
            return dbTypeName.ToLower() switch
            {
                "datetime" => "System.DateTime",
                "nvarchar" => "System.String",
                "int" => "System.Int32",
                "bigint" => "System.Int64",
                "char" => "System.Char",
                _ => throw new NotSupportedException($"The DB type '{dbTypeName}' does not have a .NET implementation.")
            };
        }

        private static string GetConversionMethod(string dbTypeName)
        {
            return dbTypeName.ToLower() switch
            {
                "datetime" => "SafeToDateTime()",
                "nvarchar" => "SafeToString()",
                "int" => "SafeToInt32()",
                "bigint" => "SafeToInt64()",
                "char" => "SafeToChar()",
                _ => throw new NotSupportedException($"The DB type '{dbTypeName}' does not have a .NET conversion method set.")
            };
        }

        private static void CreateConfigurationFile()
        {
            if (!File.Exists(ConfigurationFilePath))
                File.Create(ConfigurationFilePath).Close();
        }
    }

    public class EntityMetadata
    {
        public string EntityName { get; set; } = string.Empty;
        public List<EntityColumnMetadata> Columns { get; set; } = new();
    }

    public class EntityColumnMetadata
    {
        public string ColumnName { get; set; } = string.Empty;
        public string ColumnType { get; set; } = string.Empty;
        public string DotNetPropertyName { get; set; } = string.Empty;
        public string DotNetPropertyType { get; set; } = string.Empty;
        public string ConversionMethod { get; set; } = string.Empty;
    }
}
