using Microsoft.Data.SqlClient;

namespace VManagement.Database.Connection
{
    public sealed class VManagementConnection : IDisposable
    {
        private readonly SqlConnection _connection;

        public VManagementConnection()
        {
            if (Security.InTransaction)
            {
                _connection = VManagementTransaction.CurrentConnection!;
            }
            else
            {
                _connection = new SqlConnection(Security.ConnectionString);
                _connection.Open();
            }
        }

        public VManagementCommand CreateCommand()
        {
            return new VManagementCommand(_connection);
        }

        public void Dispose()
        {
            if (Security.InTransaction)
                return;

            if (_connection?.State == System.Data.ConnectionState.Open)
            {
                _connection?.Close();
            }

            _connection?.Dispose();
        }
    }
}
