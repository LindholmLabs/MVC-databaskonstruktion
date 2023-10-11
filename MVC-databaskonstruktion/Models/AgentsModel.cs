using MVC_databaskonstruktion.Utils;
using System.Data;

namespace MVC_databaskonstruktion.Models
{
    public class AgentsModel
    {
        private IConfiguration _configuration { get; set;}
        private DatabaseRepository _databaseRepository { get; set; }
        private TableObjectBuilder _tableBuilder { get; set;}

        public AgentsModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _databaseRepository = new DatabaseRepository(_configuration);
            _tableBuilder = new TableObjectBuilder();
        }

        public TableObject GetAgents()
        {   
            return _tableBuilder.SetControllerName("Agents")
                .SetDeleteTable("Agent")
                .SetDataTable(_databaseRepository.GetTable("Agent"))
                .SetPrimaryKeys(new List<string> { "AgentId" })
                .SetRedirect("Index")
                .Build();
        }

        public TableObject GetFieldAgents()
        {
            return _tableBuilder
                .SetDataTable(_databaseRepository.GetTable("FieldAgents"))
                .Build();
        }

        public TableObject GetGroupLeaders()
        {
            return _tableBuilder
                .SetDataTable(_databaseRepository.GetTable("GroupLeaders"))
                .Build();
        }

        public TableObject GetManagers()
        {
            return _tableBuilder
                .SetDataTable(_databaseRepository.GetTable("Managers"))
                .Build();
        }
    }
}
