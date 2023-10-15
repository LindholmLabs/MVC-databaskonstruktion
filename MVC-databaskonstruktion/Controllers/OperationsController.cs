using Microsoft.AspNetCore.Mvc;
using MVC_databaskonstruktion.Models;
using MySql.Data.MySqlClient;
using System.Numerics;

namespace MVC_databaskonstruktion.Controllers
{
    public class OperationsController : Controller
    {
        private readonly IConfiguration _configuration;
        private OperationsModel _operationsModel { get; set; }

        public OperationsController(IConfiguration configuration)
        {
            this._configuration = configuration;
            _operationsModel = new OperationsModel(_configuration);
        }

        public IActionResult Index(string OperationName = "")
        {
            ViewBag.Table = _operationsModel.GetOperations(OperationName);
            ViewBag.Modal = _operationsModel.CreateOperationModal();

            return View();
        }

        public IActionResult Details(string OperationName, DateTime StartDate, string IncidentName, int IncidentNumber)
        {
            ViewBag.AgentsInOperation = _operationsModel.GetAgentsInOperation(OperationName, StartDate, IncidentName, IncidentNumber);

            return View("Details");
        }

        public IActionResult Delete(string table, string IncidentName, string IncidentNumber, string OperationName, DateTime StartDate)
        {
            try
            {
                _operationsModel.DeleteOperation(table, IncidentName, IncidentNumber, OperationName, StartDate);
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
                        TempData["ErrorMessage"] = $"Something went wrong: {e.Number}";
                        break;
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Create(string OperationName, DateTime StartDate, DateTime EndDate, bool SuccessRate, string GroupLeader, string Incident)
        {
            try
            {
                _operationsModel.CreateOperation(OperationName, StartDate, EndDate, SuccessRate, GroupLeader, Incident);
            }
            catch (MySqlException e)
            {
                switch (e.Number)
                {
                    case 1062:
                        TempData["ErrorMessage"] = "Operation already exists!";
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
    }
}
