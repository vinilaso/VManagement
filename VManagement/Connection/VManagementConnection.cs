using Microsoft.Data.SqlClient;

namespace VManagement.Database.Connection
{
    public sealed class VManagementConnection : IDisposable
    {
        private readonly SqlConnection _connection;
        private readonly bool _ownsConnection;

        public VManagementConnection()
        {
            if (TransactionScopeManager.Current != null)
            {
                _connection = TransactionScopeManager.Current.Connection;
                _ownsConnection = false;
            }
            else
            {
                _connection = new SqlConnection(Security.ConnectionString);
                _connection.Open();
                _ownsConnection = true;
            }
        }

        public VManagementCommand CreateCommand()
        {
            return new VManagementCommand(_connection);
        }

        public void Dispose()
        {
            if (_ownsConnection)
            {
                _connection.Dispose();
            }
        }
    }
}
