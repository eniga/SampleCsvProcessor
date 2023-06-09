using Npgsql;
using System.Data;
using System.Data.SqlClient;

namespace SampleCsvProcessor
{
    public interface IDbConnectionStringProvider
    {
        Task<string> GetConnectionStringAsync();
        Task<IDbConnection> GetDbConnectionAsync();
    }

    public class DbConnectionStringProvider : IDbConnectionStringProvider
    {
        public async Task<string> GetConnectionStringAsync()
        {
            var connectionString = new NpgsqlConnectionStringBuilder
            {
                Host = "192.168.174.129",
                Port = 5432,
                Database = "default_database",
                Username = "dbuser",
                Password = "password",
                TrustServerCertificate = true,
                SslMode = SslMode.Prefer,
                Pooling = true,
                ConnectionLifetime = 180
            };
            return await Task.FromResult(connectionString.ConnectionString);
        }

        public async Task<IDbConnection> GetDbConnectionAsync()
        {
            IDbConnection connection = new NpgsqlConnection(await GetConnectionStringAsync());
            connection.Open();
            return connection;
        }
    }
}
