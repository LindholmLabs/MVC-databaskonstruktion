using MVC_databaskonstruktion.Utils;

namespace MVC_databaskonstruktion.Models
{
    public class OperatesInModel
    {
        private IConfiguration _configuration { get; set; }
        private DatabaseRepository _databaseRepository { get; set; }

        public OperatesInModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _databaseRepository = new DatabaseRepository(_configuration);
        }

        public void DeleteAgentFromOperation(string OperationName, DateTime StartDate, string IncidentName, int IncidentNumber, string CodeName)
        {
            var operatesInData = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("CodeName", CodeName),
                new KeyValuePair<string, object>("OperationName", OperationName),
                new KeyValuePair<string, object>("StartDate", StartDate.ToString("yyyy-M-dd")),
                new KeyValuePair<string, object>("IncidentName", IncidentName),
                new KeyValuePair<string, object>("IncidentNumber", IncidentNumber)
            };

            _databaseRepository.DeleteRow("OperatesIn", operatesInData);
        }
    }
}
