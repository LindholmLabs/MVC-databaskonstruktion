using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_databaskonstruktion.Utils;
using System.Diagnostics;

namespace MVC_databaskonstruktion.Models
{
    public class OperationsModel
    {

        private IConfiguration _configuration { get; set; }
        private DatabaseRepository _databaseRepository { get; set; }
        private TableObjectBuilder _tableBuilder { get; set; }

        public OperationsModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _databaseRepository = new DatabaseRepository(_configuration);
            _tableBuilder = new TableObjectBuilder()
                .SetPrimaryKeys(new List<string> { "IncidentName", "IncidentNumber", "OperationName", "StartDate" })
                .SetDeleteTable("Operation");
        }

        public TableObject GetOperations(string searchQuery)
        {
            return _tableBuilder
                .SetDataTable(_databaseRepository.GetTable($"SELECT * FROM Operation WHERE OperationName LIKE '%{searchQuery}%';"))
                .SetRedirect("Details")
                .Build();
        }

        public TableObject GetOperations(string IncidentName, int IncidentNumber)
        {
            return _tableBuilder
                .SetDataTable(_databaseRepository.GetTable($"SELECT * FROM Operation WHERE IncidentName = '{IncidentName}' AND IncidentNumber = '{IncidentNumber}';"))
                .Build();
        }

        public TableObject FilterOperations(DateTime StartDate, DateTime EndDate)
        {
            return _tableBuilder
                .SetDataTable(_databaseRepository.GetTable($"CALL GetOperationsInRange('{StartDate}', '{EndDate}')"))
                .Build();
        }

        public TableObject GetAgentsInOperation(string OperationName, DateTime StartDate, string IncidentName, int IncidentNumber)
        {
            return _tableBuilder
                .SetDataTable(_databaseRepository.GetTable($"SELECT * FROM OperatesIn WHERE OperationName = '{OperationName}' AND StartDate = '{StartDate}' AND IncidentName = '{IncidentName}' AND IncidentNumber = '{IncidentNumber}';"))
                .SetControllerName("Agents")
                .SetDeleteTable("OperatesIn")
                .SetDeletionController("OperatesIn")
                .SetRedirect("FieldAgentDetails")
                .SetPrimaryKeys(new List<string> { "CodeName", "OperationName", "StartDate", "IncidentName", "IncidentNumber" })
                .Build();
        }

        public void DeleteOperation(string table, string IncidentName, string IncidentNumber, string OperationName, DateTime StartDate)
        {
               List<KeyValuePair<string, object>> conditions = new List<KeyValuePair<string, object>>
               {
                    new KeyValuePair<string, object>("IncidentName", IncidentName),
                    new KeyValuePair<string, object>("IncidentNumber", IncidentNumber),
                    new KeyValuePair<string, object>("OperationName", OperationName),
                    new KeyValuePair<string, object>("StartDate", StartDate.ToString("yyyy-M-dd"))
               };

            _databaseRepository.DeleteRow(table, conditions);
        }

        public ModalContext CreateOperationModal()
        {
            var incidentSelection = _databaseRepository.GetColumnAsDropdown("SELECT IncidentName, IncidentNumber FROM Incident;");
            var groupLeaderSelection = _databaseRepository.GetColumnAsDropdown("SELECT CodeName FROM GroupLeaders");
            var successSelection = new List<SelectListItem>
            {
                new SelectListItem { Text = "Success", Value = "True" },
                new SelectListItem { Text = "Fail", Value = "False" }
            };

            var modalBuilder = new ModalBuilder()
                .SetTitle("Create Operation")
                .SetIdentifier("createOperationModal")
                .SetAction("Create", "Operations")
                .AddInput("OperationName", "OperationName", "normal", "OperationName")
                .AddInput("StartDate", "StartDate", "datetime", "StartDate")
                .AddInput("EndDate", "EndDate", "datetime", "EndDate")
                .AddInput("SuccessRate", "Operation Result", "dropdown", "", successSelection)
                .AddInput("GroupLeader", "GroupLeader", "dropdown", "", groupLeaderSelection)
                .AddInput("Incident", "Incident", "dropdown", "", incidentSelection);

            return modalBuilder.Build();
        }

        public void CreateOperation(string OperationName, DateTime StartDate, DateTime EndDate, bool SuccessRate, string GroupLeader, string Incident)
        {
            Trace.WriteLine("In Model: Successrate read as: " + SuccessRate);

            var IncidentName = Incident.Split(", ")[0];
            var IncidentNumber = Incident.Split(", ")[1];

            var OperationData = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("OperationName", OperationName),
                new KeyValuePair<string, object>("StartDate", StartDate.ToString("yyyy-M-d")),
                new KeyValuePair<string, object>("EndDate", EndDate.ToString("yyyy-M-d")),
                new KeyValuePair<string, object>("SuccessRate", SuccessRate),
                new KeyValuePair<string, object>("GroupLeader", GroupLeader),
                new KeyValuePair<string, object>("IncidentName", IncidentName),
                new KeyValuePair<string, object>("IncidentNumber", IncidentNumber)
            };

            _databaseRepository.CreateRow("Operation", OperationData);
        }

        public void AddAgentToOperation(string OperationName, DateTime StartDate, string IncidentName, int IncidentNumber, string CodeName)
        {
            var AgentData = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("CodeName", CodeName),
                new KeyValuePair<string, object>("OperationName", OperationName),
                new KeyValuePair<string, object>("StartDate", StartDate.ToString("yyyy-M-d")),
                new KeyValuePair<string, object>("IncidentName", IncidentName),
                new KeyValuePair<string, object>("IncidentNumber", IncidentNumber)
            };

            foreach(var data in AgentData)
            {
                Trace.WriteLine(data.Key + " : " + data.Value);
            }

            _databaseRepository.CreateRow("OperatesIn", AgentData);
        }

        public ModalContext CreateAgentModal(string OperationName, DateTime StartDate, string IncidentName, int IncidentNumber)
        {
            var fieldAgentsSelection = _databaseRepository.GetColumnAsDropdown("SELECT CodeName FROM FieldAgents");

            var modalBuilder = new ModalBuilder()
               .SetTitle("Add Agent")
               .SetIdentifier("AddAgent")
               .SetAction("AddAgent", "Operations")
               .AddInput("CodeName", "CodeName", "dropdown", "", fieldAgentsSelection)
               .AddInput("OperationName", "OperationName", "hidden", OperationName)
               .AddInput("StartDate", "StartDate", "hidden", StartDate.ToString("yyyy-M-d"))
               .AddInput("IncidentName", "IncidentName", "hidden", IncidentName)
               .AddInput("IncidentNumber", "IncidentNumber", "hidden", IncidentNumber.ToString());

            return modalBuilder.Build();
        }
    }
}