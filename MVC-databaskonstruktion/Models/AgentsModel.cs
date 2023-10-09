using MySql.Data.MySqlClient;
using Org.BouncyCastle.Security;
using System.Data;

namespace MVC_databaskonstruktion.Models
{
    public class AgentsModel
    {
        private IConfiguration _configuration;
        private string connectionString;

        public AgentsModel(IConfiguration configuration)
        {
            _configuration = configuration;
            this.connectionString = _configuration["ConnectionString"];
        }

        public DataTable GetAgents()
        {
            MySqlConnection dbcon = new MySqlConnection(connectionString);
            dbcon.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM Agent;", dbcon);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "result");
            DataTable agentsTable = ds.Tables["result"];
            dbcon.Close();

            return agentsTable;
        }
    }
}
