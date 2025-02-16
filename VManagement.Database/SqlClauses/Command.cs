using Microsoft.Data.SqlClient;
using VManagement.Database.Connection;

namespace VManagement.Database.SqlClauses
{
    public class Command
    {
        public static void Execute(string commandText)
        {
            var transaction = TransactionScope.Current;

            if (transaction != null)
            {
                var command = transaction.Connection.CreateCommand();
                command.Transaction = transaction.Transaction;
                command.CommandText = commandText;
                command.ExecuteNonQuery();
            }
            else
            {
                using (var connection = new VManagementConnection())
                {
                    var command = connection.CreateCommand();
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }
            }
        }

        internal static long ExecuteInsert(string commandText)
        {
            var transaction = TransactionScope.Current;
            SqlCommand command;

            if (transaction != null)
            {
                command = transaction.Connection.CreateCommand();
                command.Transaction = transaction.Transaction;
                command.CommandText = commandText;
                
                using var reader = command.ExecuteReader();
                if (reader.Read())
                    return reader.GetInt64(0);
            }
            else
            {
                using var connection = new VManagementConnection();
                command = connection.CreateCommand();
                command.CommandText = commandText;

                using var reader = command.ExecuteReader();
                if (reader.Read())
                    return reader.GetInt64(0);
            }

            throw new OperationCanceledException("The reader was empty.");
        }
    }
}
