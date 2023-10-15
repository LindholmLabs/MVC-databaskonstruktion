using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
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

        public void CreateAgent(
            string CodeName, 
            string FirstName, 
            string LastName, 
            decimal Salary, 
            bool IsFieldAgent, 
            bool IsGroupLeader, 
            bool IsManager)
        {
            var agentData = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("CodeName", CodeName),
                new KeyValuePair<string, object>("FirstName", FirstName),
                new KeyValuePair<string, object>("LastName", LastName),
                new KeyValuePair<string, object>("Salary", Salary),
                new KeyValuePair<string, object>("IsFieldAgent", IsFieldAgent),
                new KeyValuePair<string, object>("IsGroupLeader", IsGroupLeader),
                new KeyValuePair<string, object>("IsManager", IsManager)
            };

            _databaseRepository.CreateRow("Agent", agentData);
        }

        public void DeleteAgent(string table, string CodeName)
        {
            List<KeyValuePair<string, string>> conditions = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("CodeName", CodeName)
            };

            _databaseRepository.DeleteRow(table, conditions);
        }

        public TableObject SearchAgents(string searchQuery)
        {
            return _tableBuilder
                .SetDataTable(_databaseRepository.GetTable($"SELECT * FROM Agent WHERE CodeName LIKE '%{searchQuery}%' OR FirstName LIKE '%{searchQuery}%' OR LastName LIKE '%{searchQuery}%';"))
                .Build();
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
                .SetControllerName("Operations")
                .SetPrimaryKeys(new List<string> { "OperationName", "StartDate", "IncidentName", "IncidentNumber"})
                .SetDeleteTable("Operation")
                .SetRedirect("Details")
                .Build();
        }

        public ModalContext CreateAgentModal()
        {
            var trueFalseList = new List<SelectListItem>
            {
                new SelectListItem { Text = "True", Value = "True" },
                new SelectListItem { Text = "False", Value = "False" }
            };

            var modalBuilder = new ModalBuilder()
               .SetTitle("Hire Agent")
               .SetIdentifier("AddAgent")
               .SetAction("Create", "Agents")
               .AddInput("CodeName", "CodeName", "normal", "CodeName")
               .AddInput("FirstName", "FirstName", "normal", "firstname")
               .AddInput("LastName", "LastName", "normal", "lastname")
               .AddInput("Salary", "Salary", "normal", "Salary")
               .AddInput("IsFieldAgent", "IsFieldAgent", "dropdown", "", trueFalseList)
               .AddInput("IsGroupLeader", "IsGroupLeader", "dropdown", "", trueFalseList)
               .AddInput("IsManager", "IsManager", "dropdown", "", trueFalseList);

            return modalBuilder.Build();
        }
    }
}
