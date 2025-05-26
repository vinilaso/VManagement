using Microsoft.Data.SqlClient;

namespace VManagement.Database.Connection
{
    public sealed class VManagementTransaction : IDisposable
    {
        private enum TransactionState
        {
            Complete,
            Incomplete
        }

        private static AsyncLocal<VManagementTransaction?> _current = new AsyncLocal<VManagementTransaction?>();
        internal static SqlConnection? CurrentConnection => _current?.Value?._connection;
        internal static SqlTransaction? CurrentTransaction => _current?.Value?._transaction;

        private readonly SqlConnection _connection;
        private readonly SqlTransaction _transaction;
        private bool _commited = false;
        private TransactionState _state = TransactionState.Incomplete;

        public static VManagementTransaction? Current => _current.Value;

        public VManagementTransaction()
        {
            if (Current != null)
            {
                _connection = CurrentConnection!;
                _transaction = CurrentTransaction!;
            }
            else
            {
                _connection = new SqlConnection(Security.ConnectionString);
                _connection.Open();
                _transaction = _connection.BeginTransaction();

                _current.Value = this;
            }
        }

        public void Complete()
        {
            _state = TransactionState.Complete;
        }

        public void Dispose()
        {
            if (_state == TransactionState.Complete)
            {
                Commit();
            }
            else
            {
                Rollback();
            }

            _transaction?.Dispose();
            _connection?.Dispose();
            _current.Value = null;
        }

        private void Commit()
        {
            if (!_commited)
            {
                _transaction.Commit();
                _commited = true;
            }
        }

        private void Rollback()
        {
            if (!_commited)
            {
                _transaction.Rollback();
                _commited = true;
            }
        }
    }
}
