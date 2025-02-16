using Microsoft.Data.SqlClient;

namespace VManagement.Database.Connection
{
    /// <summary>
    /// This class is used to provide the application connections to the database.
    ///     <remarks>
    ///         You'll always want to use it with a using clause.
    ///     </remarks>
    /// </summary>
    public sealed class VManagementConnection : IDisposable
    {
        private readonly SqlConnection _connection;

        internal SqlConnection Connection => _connection;

        public SqlCommand CreateCommand()
        {
            return _connection.CreateCommand();
        }

        public VManagementConnection()
        {
            _connection = new SqlConnection(Security.Instance.ConnectionString);
            _connection.Open();
        }

        public void Dispose()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }

            _connection.Dispose();
        }
    }
}
