using Microsoft.Data.SqlClient;

namespace VManagement.Database.Connection
{
    public sealed class VManagementTransaction : IDisposable
    {
        private readonly SqlConnection _connection;
        private readonly SqlTransaction _transaction;
        private bool _isCompleted = false;

        internal SqlConnection Connection => _connection;
        internal SqlTransaction Transaction => _transaction;

        public VManagementTransaction()
        {
            _connection = new SqlConnection(Security.ConnectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();

            // Ao ser criada, se registra como a transação ambiente atual.
            TransactionScopeManager.Push(this);
        }

        public void Complete()
        {
            _isCompleted = true;
        }

        public void Dispose()
        {
            try
            {
                if (_isCompleted)
                {
                    _transaction.Commit();
                }
                else
                {
                    _transaction.Rollback();
                }
            }
            finally
            {
                _transaction.Dispose();
                _connection.Dispose();

                TransactionScopeManager.Pop();
            }
        }
    }
}
