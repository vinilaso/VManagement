using Microsoft.Data.SqlClient;
using VManagement.Commons.Entities;
using VManagement.Database.Clauses;

namespace VManagement.Database.Connection
{
    public sealed class VManagementCommand
    {
        public string CommandText
        {
            get => _command.CommandText;
            set => _command.CommandText = value;
        }
        public SqlParameterCollection Parameters
        {
            get => _command.Parameters;
        }

        private readonly SqlCommand _command;
        private readonly SqlConnection _connection;

        internal VManagementCommand(SqlConnection connection)
        {
            _connection = connection;
            _command = _connection.CreateCommand();
            _command.Transaction = VManagementTransaction.CurrentTransaction;
        }

        public void SetParameters(ParameterCollection parameters)
        {
            parameters.ForEach(param => _command.Parameters.Add(param));
        }

        public void SetParameters(CoreEntity entity)
        {
            foreach (var field in entity.Fields.Where(f => f.Name != "ID"))
            {
                object value = field.Value ?? DBNull.Value;
                _command.Parameters.Add(new SqlParameter($"@{field.Name}", value));
            }
        }

        public void ExecuteNonQuery()
        {
            _command.ExecuteNonQuery();
        }

        public SqlDataReader ExecuteReader()
        {
            return _command.ExecuteReader();
        }

        public TGeneric ExecuteScalar<TGeneric>()
        {
            return (TGeneric)_command.ExecuteScalar();
        }
    }
}
