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
            ViewBag.Modal = _incidentsModel.CreateIncidentModal();
            
            return View();
        }

        public IActionResult Details(string IncidentName, int incidentNumber)
        {
            ViewBag.Operations = _incidentsModel.GetOperations(IncidentName, incidentNumber);
            ViewBag.AddOperationModal = _incidentsModel.CreateOperationModal(IncidentName, incidentNumber);
            ViewBag.Reports = _incidentsModel.GetReports(IncidentName, incidentNumber);

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

        public IActionResult Create(string IncidentName, int IncidentNumber, string RegionName, int Terrain, string Location)
        {
            try
            {
                _incidentsModel.CreateIncident(IncidentName, IncidentNumber, RegionName, Terrain, Location);
            }
            catch (MySqlException e)
            {
                switch (e.Number)
                {
                    case 1062:
                        TempData["ErrorMessage"] = "Incident already exists!";
                        break;
                    case 1406:
                        TempData["ErrorMessage"] = "Data too long!";
                        break;
                    case 3819:
                        TempData["ErrorMessage"] = "Invalid Data!";
                        break;
                    default:
                        TempData["ErrorMessage"] = $"Something went wrong: {e.Number}";
                        break;
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult CreateOperation(string OperationName, DateTime StartDate, DateTime EndDate, bool SuccessRate, string GroupLeader, string IncidentName, int IncidentNumber)
        {
            try
            {
                _incidentsModel.CreateOperation(OperationName, StartDate, EndDate, SuccessRate, GroupLeader, IncidentName, IncidentNumber);
            }
            catch (MySqlException e)
            {
                switch (e.Number)
                {
                    case 1062:
                        TempData["ErrorMessage"] = "Incident already exists!";
                        break;
                    case 1406:
                        TempData["ErrorMessage"] = "Data too long!";
                        break;
                    case 3819:
                        TempData["ErrorMessage"] = "Invalid Data!";
                        break;
                    default:
                        TempData["ErrorMessage"] = $"Something went wrong: {e.Number}";
                        break;
                }
            }

            return RedirectToAction("Details", new
            {
                IncidentName,
                IncidentNumber
            });
        }
    }
}
