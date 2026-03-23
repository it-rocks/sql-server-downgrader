using System.Data;
using System.Data.SqlClient;
using SqlServerDowngrader.Models;

namespace SqlServerDowngrader.Services
{
    public class DatabaseConnectionService
    {
        public List<string> GetDatabases(string instanceName)
        {
            var databases = new List<string>();

            try
            {
                string connectionString = BuildConnectionString(instanceName, "master");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT name FROM sys.databases WHERE database_id > 4 ORDER BY name";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                databases.Add(reader["name"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving databases: {ex.Message}");
            }

            return databases;
        }

        public DataTable GetDatabaseSchema(string instanceName, string databaseName)
        {
            var schemaTable = new DataTable();

            try
            {
                string connectionString = BuildConnectionString(instanceName, databaseName);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            TABLE_SCHEMA, 
                            TABLE_NAME, 
                            TABLE_TYPE 
                        FROM INFORMATION_SCHEMA.TABLES 
                        ORDER BY TABLE_SCHEMA, TABLE_NAME";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(schemaTable);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving schema: {ex.Message}");
            }

            return schemaTable;
        }

        public bool TestConnection(string instanceName)
        {
            try
            {
                string connectionString = BuildConnectionString(instanceName, "master");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return connection.State == ConnectionState.Open;
                }
            }
            catch
            {
                return false;
            }
        }

        private string BuildConnectionString(string instanceName, string databaseName)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = instanceName,
                InitialCatalog = databaseName,
                IntegratedSecurity = true,
                ConnectTimeout = 10,
                Encrypt = false
            };

            return builder.ConnectionString;
        }
    }
}