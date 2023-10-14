using Microsoft.AspNetCore.Mvc;
using MVC_databaskonstruktion.Models;
using MySql.Data.MySqlClient;

namespace MVC_databaskonstruktion.Controllers
{
    public class IncidentsController : Controller
    {
        private readonly IConfiguration _configuration;
        private IncidentsModel _incidentsModel { get; set; }

        public IncidentsController(IConfiguration configuration)
        {
            this._configuration = configuration;
            _incidentsModel = new IncidentsModel(_configuration);
        }

        public IActionResult Index(string IncidentName = "")
        {
            ViewBag.Table = _incidentsModel.GetIncidents(IncidentName);

            return View();
        }

        public IActionResult Details(string IncidentName, int incidentNumber)
        {
            ViewBag.Operations = _incidentsModel.GetOperations(IncidentName, incidentNumber);
            return View();
        }

        public IActionResult Delete(string table, string incidentName, string incidentNumber)
        {
            try
            {
                _incidentsModel.DeleteIncident(table, incidentName, incidentNumber);
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

            return RedirectToAction("Index");
        }
    }
}
