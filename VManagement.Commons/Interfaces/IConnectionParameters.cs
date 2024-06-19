using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VManagement.Commons.Interfaces
{
    public interface IConnectionParameters
    {
        public string DataSource { get; set; } 
        public string InitialCatalog { get; set; } 
        public string UserId { get; set; } 
        public string Password { get; set; } 
    }
}
