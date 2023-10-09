using MySql.Data.MySqlClient;
using Org.BouncyCastle.Security;
using System.Data;

namespace MVC_databaskonstruktion.Models
{
    public class AgentsModel
    {
        private IConfiguration _configuration;
        private string connectionString;
        private DatabaseRepository _databaseRepository;

        public AgentsModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _databaseRepository = new DatabaseRepository(_configuration);
        }

        public DataTable GetAgents()
        {
            return _databaseRepository.GetTable("Agent");
        }

        public DataTable GetFieldAgents()
        {
            return _databaseRepository.GetTable("FieldAgents");
        }

        public DataTable GetGroupLeaders()
        {
            return _databaseRepository.GetTable("GroupLeaders");
        }

        public DataTable GetManagers()
        {
            return _databaseRepository.GetTable("Managers");
        }
    }
}
