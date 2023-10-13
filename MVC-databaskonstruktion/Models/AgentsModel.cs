using Microsoft.AspNetCore.Mvc.Rendering;
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
                .SetDeleteTable("Agent");
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
                .SetRedirect("FieldAgentDetails")
                .Build();
        }

        public TableObject GetGroupLeaders()
        {
            return _tableBuilder
                .SetDataTable(_databaseRepository.GetTable("GroupLeaders"))
                .SetRedirect(string.Empty)
                .Build();
        }

        public TableObject GetManagers()
        {
            return _tableBuilder
                .SetDataTable(_databaseRepository.GetTable("Managers"))
                .SetRedirect(string.Empty)
                .Build();
        }

        public TableObject GetAgentOperations(string CodeName)
        {

            string query = $"SELECT * FROM OperatesIn WHERE CodeName = '{CodeName}';";
            return _tableBuilder.SetDataTable(_databaseRepository.GetTable(query))
                .SetDeleteTable(string.Empty)
                .SetRedirect(string.Empty)
                .Build();
        }

        public Modal CreateAgentModal()
        {
            var trueFalseList = new List<SelectListItem>
            {
                new SelectListItem { Text = "True", Value = "True" },
                new SelectListItem { Text = "False", Value = "False" }
            };

            var modalBuilder = new ModalBuilder()
               .WithTitle("AddAgent")
               .AddInput("CodeName", "CodeName", "normal", "CodeName")
               .AddInput("FirstName", "FirstName", "normal", "firstname")
               .AddInput("LastName", "LastName", "normal", "lastname")
               .AddInput("Salary", "Salary", "normal", "Salary")
               .AddInput("IsFieldAgent", "Choose Option", "dropdown", "", trueFalseList)
               .AddInput("IsGroupLeader", "Choose Option", "dropdown", "", trueFalseList)
               .AddInput("IsManager", "Choose Option", "dropdown", "", trueFalseList);

            return modalBuilder.Build();
        }
    }
}
