using Microsoft.AspNetCore.Mvc;
using MVC_databaskonstruktion.Models;
using MVC_databaskonstruktion.Utils;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace MVC_databaskonstruktion.Controllers
{
    public class AgentsController : Controller
    {
        private readonly IConfiguration _configuration;
        private AgentsModel _agentsModel { get; set; }

        public AgentsController(IConfiguration configuration) 
        {
            this._configuration = configuration;
            _agentsModel = new AgentsModel(_configuration);
        }

        public IActionResult Index()
        {
            MapTables();
            MapDropdowns();

            return View();
        }

        public IActionResult Create(
            string CodeName, 
            string FirstName, 
            string LastName, 
            decimal Salary, 
            bool IsFieldAgent, 
            bool IsGroupLeader, 
            bool IsManager)
        {
            Trace.WriteLine("Running function: Create new Agent." +
                "Received parameters: " +
                CodeName + FirstName + LastName + Salary + IsFieldAgent + IsGroupLeader + IsManager);

            try
            {
                _agentsModel.CreateAgent(CodeName, FirstName, LastName, Salary, IsFieldAgent, IsGroupLeader, IsManager);
            }
            catch (MySqlException e)
            {
                switch (e.Number)
                {
                    case 1062:
                        TempData["ErrorMessage"] = "CodeName already exists!";
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

        public IActionResult FieldAgentDetails(string CodeName)
        {
            ViewBag.AgentOperations = _agentsModel.GetAgentOperations(CodeName);
            return View();
        }

        public IActionResult Delete(string table, string CodeName)
        {
            try
            {
                _agentsModel.DeleteAgent(table, CodeName);
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

        private void MapTables()
        {
            ViewBag.FieldAgents = _agentsModel.GetFieldAgents();
            ViewBag.GroupLeaders = _agentsModel.GetGroupLeaders();
            ViewBag.Managers = _agentsModel.GetManagers();
        }
        
        public void MapDropdowns()
        {
            ViewBag.CreateAgentModal = _agentsModel.CreateAgentModal();
        }
    }
}