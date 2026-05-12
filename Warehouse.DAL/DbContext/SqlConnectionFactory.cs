using Microsoft.Extensions.Configuration;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace Warehouse.DAL.DbContext
{
    public class SqlConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")
            );
        }
    }
}