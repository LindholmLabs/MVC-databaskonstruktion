using Microsoft.AspNetCore.Mvc;
using MVC_databaskonstruktion.Models;
using MySql.Data.MySqlClient;
using System.Linq.Expressions;

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
            BuildAgentTables();
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
                    case 1062:
                        TempData["ErrorMessage"] = "Unique Constraint Failed!";
                        break;
                    default:
                        TempData["ErrorMessage"] = $"Something went wrong: {e.Number}";
                        break;
                }
            }
            return RedirectToAction("Index");
        }

        private void BuildAgentTables()
        {
            ViewBag.FieldAgents = _agentsModel.GetFieldAgents();
            ViewBag.GroupLeaders = _agentsModel.GetGroupLeaders();
            ViewBag.Managers = _agentsModel.GetManagers();
        }
    }
}