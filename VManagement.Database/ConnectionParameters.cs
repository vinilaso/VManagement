using VManagement.Commons.Interfaces;

namespace VManagement.Database
{
    public class ConnectionParameters : IConnectionParameters
    {
        public string DataSource { get; set; } = string.Empty;
        public string InitialCatalog { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
