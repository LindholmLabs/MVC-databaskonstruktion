using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_databaskonstruktion.Utils;

namespace MVC_databaskonstruktion.Models
{
    public class IncidentsModel
    {
        private IConfiguration _configuration { get; set; }
        private DatabaseRepository _databaseRepository { get; set; }
        private TableObjectBuilder _tableBuilder { get; set; }

        public IncidentsModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _databaseRepository = new DatabaseRepository(_configuration);
            _tableBuilder = new TableObjectBuilder()
                .SetPrimaryKeys(new List<string> { "IncidentName", "IncidentNumber" })
                .SetDeleteTable("Incident");
        }

        public TableObject GetIncidents(string searchQuery)
        {
            return _tableBuilder
                .SetDataTable(_databaseRepository.GetTable($"SELECT * FROM Incident WHERE IncidentName LIKE '%{searchQuery}%';"))
                .SetRedirect("Details")
                .Build();
        }

        public TableObject GetReports(string IncidentName, int IncidentNumber)
        {
            var reportsTable = new TableObjectBuilder();

            return reportsTable
                .SetDataTable(_databaseRepository.GetTable($"SELECT * FROM Report WHERE IncidentName = '{IncidentName}' AND IncidentNumber = '{IncidentNumber}';"))
                .Build();
        }

        public void DeleteIncident(string table, string incidentName, string incidentNumber)
        {
            List<KeyValuePair<string, object>> conditions = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("IncidentName", incidentName),
                new KeyValuePair<string, object>("IncidentNumber", incidentNumber)
            };

            _databaseRepository.DeleteRow(table, conditions);
        }

        public TableObject GetOperations(string IncidentName, int IncidentNumber)
        {
            return _tableBuilder
                .SetDataTable(_databaseRepository.GetTable($"SELECT * FROM Operation WHERE IncidentName = '{IncidentName}' AND IncidentNumber = '{IncidentNumber}';"))
                .SetRedirect("Details")
                .SetDeleteTable("Operation")
                .SetPrimaryKeys(new List<string> { "IncidentName", "IncidentNumber", "OperationName", "StartDate" })
                .SetControllerName("Operations")
                .Build();
        }

        public ModalContext CreateIncidentModal()
        {
            var TerrainSelection = _databaseRepository.GetColumnAsDropdown("SELECT TerrainCode FROM Terrain;");

            var modalBuilder = new ModalBuilder()
                .SetTitle("Create Incident")
                .SetIdentifier("createIncidentModal")
                .SetAction("Create", "Incidents")
                .AddInput("IncidentName", "IncidentName", "normal", "IncidentName")
                .AddInput("IncidentNumber", "IncidentNumber", "normal", "IncidentNumber")
                .AddInput("RegionName", "RegionName", "normal", "RegionName")
                .AddInput("Terrain", "Terrain", "dropdown", "", TerrainSelection)
                .AddInput("Location", "Location", "normal", "Location");

            return modalBuilder.Build();
        }

        public ModalContext CreateOperationModal(string IncidentName, int IncidentNumber)
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
                .SetAction("CreateOperation", "Incidents")
                .AddInput("OperationName", "OperationName", "normal", "OperationName")
                .AddInput("StartDate", "StartDate", "datetime", "StartDate")
                .AddInput("EndDate", "EndDate", "datetime", "EndDate")
                .AddInput("SuccessRate", "Operation Result", "dropdown", "", successSelection)
                .AddInput("GroupLeader", "GroupLeader", "dropdown", "", groupLeaderSelection)
                .AddInput("IncidentName", "IncidentName", "hidden", IncidentName)
                .AddInput("IncidentNumber", "IncidentNumber", "hidden", IncidentNumber.ToString());

            return modalBuilder.Build();
        }

        public void CreateIncident(string IncidentName, int IncidentNumber, string RegionName, int Terrain, string Location)
        {
            var IncidentData = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("IncidentName", IncidentName),
                new KeyValuePair<string, object>("IncidentNumber", IncidentNumber),
                new KeyValuePair<string, object>("RegionName", RegionName),
                new KeyValuePair<string, object>("Terrain", Terrain),
                new KeyValuePair<string, object>("Location", Location)
            };

            _databaseRepository.CreateRow("Incident", IncidentData);
        }

        public void CreateOperation(string OperationName, DateTime StartDate, DateTime EndDate, bool SuccessRate, string GroupLeader, string IncidentName, int IncidentNumber)
        {
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
    }
}
