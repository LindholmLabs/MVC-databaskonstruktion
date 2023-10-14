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

        public IActionResult Index()
        {
            ViewBag.Table = _operationsModel.GetOperations();

            return View();
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
    }
}
