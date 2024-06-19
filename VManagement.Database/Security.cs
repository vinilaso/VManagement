using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using VManagement.Commons.Interfaces;

namespace VManagement.Database
{
    // A forma que a classe armazena strings de conexão com o banco é com arquivos de texto (.txt)
    // Uma melhoria futura é planejada para proporcionar mais segurança.
    public class Security
    {
        /// <summary>
        /// Caminho para o arquivo que guarda a string de conexão.
        /// </summary>
        private static string ConnStringFilePath => Path.Combine(AppContext.BaseDirectory, "ConnectionString.txt");

        /// <summary>
        /// Constrói uma conexão com o banco de dados.
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetDatabaseConnection()
        {
            return new SqlConnection(GetConnectionString());
        }

        /// <summary>
        /// Registra uma nova string de conexão no sistema.
        /// </summary>
        /// <param name="parameters">Parâmetros da conexão</param>
        public static void RegisterSqlConnection(IConnectionParameters parameters)
        {
            SqlConnectionStringBuilder builder = new();
            builder.UserID = parameters.UserId;
            builder.Password = parameters.Password;
            builder.InitialCatalog = parameters.InitialCatalog;
            builder.DataSource = parameters.DataSource;
            builder.TrustServerCertificate = true;

            ValidateConnStringFile();
            
            File.WriteAllText(ConnStringFilePath, builder.ConnectionString);
        }

        /// <summary>
        /// Retorna a String de conexão ao banco de dados
        /// </summary>
        /// <returns></returns>
        private static string GetConnectionString()
        {
            using FileStream file = File.Open(ConnStringFilePath, FileMode.Open, FileAccess.Read);
            using StreamReader reader = new(file);

            return reader.ReadToEnd();
        }

        /// <summary>
        /// Valida se o arquivo que armazena a string de conexão existe.
        /// </summary>
        private static void ValidateConnStringFile()
        {
            if (File.Exists(ConnStringFilePath))
                File.Delete(ConnStringFilePath);

            File.Create(ConnStringFilePath).Close();
        }
    }
}
