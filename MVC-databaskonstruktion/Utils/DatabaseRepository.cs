using MySql.Data.MySqlClient;
using System.Data;

namespace MVC_databaskonstruktion.Utils
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

        public void DeleteRow(string table, List<KeyValuePair<string, string>> primaryKeys)
        {
            string whereClause = "WHERE ";
            for (var i = 0; i < primaryKeys.Count; i++)
            {
                whereClause += $"{primaryKeys[i].Key} = '{primaryKeys[i].Value}'";
                if (i < primaryKeys.Count - 1)
                {
                    whereClause += " AND ";
                }
            }

            using (MySqlConnection dbcon = new MySqlConnection(_connectionString))
            {
                dbcon.Open();
                MySqlCommand cmd = new MySqlCommand($"DELETE FROM {table} {whereClause};", dbcon);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
