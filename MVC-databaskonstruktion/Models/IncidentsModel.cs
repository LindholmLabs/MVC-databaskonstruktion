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

        public void DeleteIncident(string table, string incidentName, string incidentNumber)
        {
            List<KeyValuePair<string, string>> conditions = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("IncidentName", incidentName),
                new KeyValuePair<string, string>("IncidentNumber", incidentNumber)
            };

            _databaseRepository.DeleteRow(table, conditions);
        }

        public TableObject GetOperations(string IncidentName, int IncidentNumber)
        {
            return _tableBuilder
                .SetDataTable(_databaseRepository.GetTable($"SELECT * FROM Operation WHERE IncidentName = '{IncidentName}' AND IncidentNumber = '{IncidentNumber}';"))
                .Build();
        }
    }
}
