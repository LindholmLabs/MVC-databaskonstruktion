using MVC_databaskonstruktion.Utils;

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

        public TableObject GetOperations()
        {
            return _tableBuilder
                .SetDataTable(_databaseRepository.GetTable("Operation"))
                .Build();
        }

        public TableObject GetOperations(string IncidentName, int IncidentNumber)
        {
            return _tableBuilder
                .SetDataTable(_databaseRepository.GetTable($"SELECT * FROM Operation WHERE IncidentName = '{IncidentName}' AND IncidentNumber = '{IncidentNumber}';"))
                .Build();
        }

        public void DeleteOperation(string table, string IncidentName, string IncidentNumber, string OperationName, DateTime StartDate)
        {
               List<KeyValuePair<string, string>> conditions = new List<KeyValuePair<string, string>>
               {
                    new KeyValuePair<string, string>("IncidentName", IncidentName),
                    new KeyValuePair<string, string>("IncidentNumber", IncidentNumber),
                    new KeyValuePair<string, string>("OperationName", OperationName),
                    new KeyValuePair<string, string>("StartDate", StartDate.ToString("yyyy-M-d"))
               };

            _databaseRepository.DeleteRow(table, conditions);
        }
    }
}
