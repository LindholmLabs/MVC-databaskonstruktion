using Microsoft.AspNetCore.Mvc;
using MVC_databaskonstruktion.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System.Diagnostics;

namespace MVC_databaskonstruktion.Controllers
{
    public class OperatesIn : Controller
    {
        private readonly IConfiguration _configuration;
        private OperatesInModel _operatesInModel { get; set; }

        public OperatesIn(IConfiguration configuration)
        {
            this._configuration = configuration;
            _operatesInModel = new OperatesInModel(_configuration);
        }

        public IActionResult Delete(string OperationName, DateTime StartDate, string IncidentName, int IncidentNumber, string CodeName)
        {
            try
            {
                _operatesInModel.DeleteAgentFromOperation(OperationName, StartDate, IncidentName, IncidentNumber, CodeName);
            }
            catch (MySqlException e)
            {
                switch (e.Number)
                {
                    case 1451:
                        TempData["ErrorMessage"] = "Foreign Key Constraint Failed!";
                        break;
                    case 1452:
                        TempData["ErrorMessage"] = "Foreign Key Constraint Failed!";
                        break;
                    default:
                        TempData["ErrorMessage"] = $"Something went wrong: error {e.Number}";
                        break;
                }
            }

            return RedirectToAction("Details", "Operations", new
            {
                OperationName,
                StartDate,
                IncidentName,
                IncidentNumber
            });
        }
    }
}
