using System.Text;

namespace VManagement.Analyzers.DTO
{
    public class EntityMetadata
    {
        public string EntityName { get; set; } = string.Empty;
        public EntityColumnMetadata[] Columns { get; set; } = [];

        public override string ToString()
        {
            // StringBuilder é eficiente para construir strings em loops.
            var sb = new StringBuilder();

            sb.AppendLine($"Entity: {EntityName}");

            if (Columns == null || Columns.Length == 0)
            {
                sb.AppendLine("  (No columns defined)");
            }
            else
            {
                sb.AppendLine("  Columns:");
                foreach (var column in Columns)
                {
                    // Adiciona a representação de cada coluna, com indentação.
                    // Isso chama o método ToString() da classe EntityColumnMetadata.
                    sb.AppendLine($"    - {column}");
                }
            }

            return sb.ToString().TrimEnd(); // Remove a última quebra de linha
        }
    }

    public class EntityColumnMetadata
    {
        public string ColumnName { get; set; } = string.Empty;
        public string ColumnType { get; set; } = string.Empty;

        public string DotNetPropertyName { get; set; } = string.Empty;
        public string DotNetPropertyType { get; set; } = string.Empty;
        public string ConversionMethod { get; set; } = string.Empty;

        public override string ToString()
        {
            // Formato: [Propriedade (.NET Tipo)] -> [COLUNA (SQL Tipo)]
            return $"[{DotNetPropertyName} ({DotNetPropertyType})] -> [{ColumnName} ({ColumnType})]";
        }
    }
}
