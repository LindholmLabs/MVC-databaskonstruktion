using MySql.Data.MySqlClient;
using System.Data;

namespace MVC_databaskonstruktion.Models
{
    public class DatabaseRepository
    {
        private readonly string _connectionString;

        public DatabaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionString"];
        }

        public DataTable GetTable(string query)
        {
            using (MySqlConnection dbcon = new MySqlConnection(_connectionString))
            {
                if (!query.Contains(" "))
                {
                    query = $"SELECT * FROM {query};";
                }

                dbcon.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, dbcon);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "result");
                return ds.Tables["result"];
            }
        }
    }
}
