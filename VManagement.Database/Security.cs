using Microsoft.Data.SqlClient;
using VManagement.Commons.Utility;
using VManagement.Database.Connection;

namespace VManagement.Database
{
    /// <summary>
    /// This class is responsible for the management of the connection to the database.
    /// </summary>
    public sealed class Security
    {
        private string? _connectionString;
        public string ConnectionString
        {
            get
            {
                if (_connectionString == null || _connectionString == string.Empty)
                    throw new ArgumentNullException("The connection string is not defined. Plase use the Security.SetConnectionString method in the start of your application.");

                return _connectionString;
            }
            private set
            {
                _connectionString = value;
            }
        }

        private static bool _inTransaction = false;
        public static bool InTransaction
        {
            get
            {
                return _inTransaction;
            }
            internal set
            {
                if (_inTransaction && value)
                    throw new InvalidOperationException("A transaction is already in progress. You cannot start a new one until the current transaction is completed.");

                _inTransaction = value;
            }
        }

        public static Security Instance { get; private set; }

        static Security()
        {
            Instance = new Security();
        }

        private Security() { }

        internal void SetConnectionString(string filePath)
        {
            SqlConnectionStringBuilder connectionStringBuilder = new();

            FileUtility fileUtils = new(filePath);

            connectionStringBuilder.DataSource     = fileUtils.FindValue("DataSource");
            connectionStringBuilder.InitialCatalog = fileUtils.FindValue("InitialCatalog");
            connectionStringBuilder.Password       = fileUtils.FindValue("Password");
            connectionStringBuilder.UserID         = fileUtils.FindValue("UserID");
            connectionStringBuilder.Pooling        = true;
            connectionStringBuilder.TrustServerCertificate = true;

            _connectionString = connectionStringBuilder.ToString();
        }

        public static void SetupEnvironment()
        {
            Instance.SetConnectionString("C:\\VManagement\\Files\\ConnectionString.txt");
        }

        public static bool TestConnection(out string error)
        {
            error = string.Empty;

            try
            {
                using (var connection = new VManagementConnection())
                {
                    
                }

                return true;
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }
    }
}
