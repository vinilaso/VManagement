using Microsoft.Data.SqlClient;

namespace VManagement.Database.SqlClauses
{
    public class Parameter
    {
        public string Name { get; set; } = string.Empty;
        public object? Value { get; set; }

        public Parameter(string name, object? value)
        {
            Name = name;
            Value = value;
        }

        public SqlParameter AsSqlParameter()
        {
            return new SqlParameter(Name, Value);
        }
    }
}
