using Microsoft.Data.SqlClient;

namespace VManagement.Database.Connection
{
    /// <summary>
    /// Provides a transaction context to the application during the SQL operations.
    /// </summary>
    public sealed class TransactionScope : IDisposable
    {
        public enum TransactionState
        {
            Incomplete,
            Complete
        }

        private static AsyncLocal<TransactionScope?> _current = new AsyncLocal<TransactionScope?>();

        private readonly VManagementConnection _connection;
        private readonly SqlTransaction _transaction;
        private bool _commited = false;

        public static TransactionScope? Current => _current.Value;
        public VManagementConnection Connection => _connection;
        public SqlTransaction Transaction => _transaction;
        public TransactionState State { get; private set; } = TransactionState.Incomplete;


        public TransactionScope()
        {
            _connection = new VManagementConnection();
            _transaction = _connection.Connection.BeginTransaction();

            _current.Value = this;
            Security.InTransaction = true;
        }

        public void Complete()
        {
            State = TransactionState.Complete;
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

        public void Dispose()
        {
            if (State == TransactionState.Complete)
            {
                Commit();
            }
            else
            {
                Rollback();
            }

            _transaction.Dispose();
            _connection.Dispose();

            _current.Value = null;
            Security.InTransaction = false;
        }
    }
}
