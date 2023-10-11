using MVC_databaskonstruktion.Utils;

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
            _tableBuilder = new TableObjectBuilder()
                .SetPrimaryKeys(new List<string> { "CodeName" })
                .SetDeleteTable("Agent")
                .SetRedirect("Index");
        }

        public TableObject GetAgents()
        {   
            return _tableBuilder.SetControllerName("Agents")
                .SetDataTable(_databaseRepository.GetTable("Agent"))
                .Build();
        }

        public void DeleteAgent(string table, string CodeName)
        {
            List<KeyValuePair<string, string>> conditions = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("CodeName", CodeName)
            };

            _databaseRepository.DeleteRow(table, conditions);
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
