using Microsoft.Data.SqlClient;
using VManagement.Commons.Utility;
using VManagement.Database.Connection;

namespace VManagement.Database
{
    public static class Security
    {
        private static string _internalFilesPath = string.Empty;

        public static bool InTransaction => VManagementTransaction.Current != null;
        public static string InternalFilesPath
        {
            get
            {
                if (_internalFilesPath.IsNullOrEmpty())
                    throw new InvalidOperationException("The default path for internal files is not set yet.");

                return _internalFilesPath;
            }
        }
        public static string? ConnectionString { get; set; }
        internal static string DefaultConnectionStringPath => "C:\\VManagementV2\\Files\\connectionString.txt";

        public static void SetupEnvironment()
        {
            SetConnectionString(DefaultConnectionStringPath);
            SetInternalFilesPath("C:\\VManagementV2\\Files");
        }

        public static bool TestConnection(out string error)
        {
            error = string.Empty;

            try
            {
                using SqlConnection conn = new SqlConnection(ConnectionString);
                conn.Open();
                return true;
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        internal static void SetInternalFilesPath(string internalFilesPath)
        {
            _internalFilesPath = internalFilesPath;
        }

        internal static void SetConnectionString(string connectionStringFilePath)
        {
            try
            {
                using FileStream connectionStringFile = File.OpenRead(connectionStringFilePath);
                using StreamReader reader = new StreamReader(connectionStringFile);

                string fileContent = reader.ReadToEnd();

                if (string.IsNullOrEmpty(fileContent))
                    throw new InvalidDataException($"The content in the file {connectionStringFilePath} is empty.");

                var values = fileContent.Split(';', StringSplitOptions.TrimEntries)
                                        .ToDictionary(v => v.Split('=')[0], v => v.Split('=')[1]);

                SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder()
                {
                    DataSource = values["DataSource"],
                    InitialCatalog = values["InitialCatalog"],
                    Password = values["Password"],
                    UserID = values["UserID"],
                    Pooling = true,
                    TrustServerCertificate = true
                };

                ConnectionString = connectionStringBuilder.ToString();

            }
            catch (Exception e)
            {
                throw new Exception($"Error while setting the connection string. Error: {e.Message} - Stack Trace: {e.StackTrace}", e);
            }
        }
    }
}
