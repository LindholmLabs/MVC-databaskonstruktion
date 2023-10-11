using Microsoft.AspNetCore.Mvc;
using MVC_databaskonstruktion.Models;
using MVC_databaskonstruktion.Utils;
using System.Data;

namespace MVC_databaskonstruktion.Controllers
{
    public class AgentsController : Controller
    {
        private readonly IConfiguration _configuration;
        private AgentsModel _agentsModel;

        public AgentsController(IConfiguration configuration) 
        {
            this._configuration = configuration;
        }

        public IActionResult Index()
        {
            BuildAgentTables();
            return View();
        }

        private void BuildAgentTables()
        {
            _agentsModel = new AgentsModel(this._configuration);

            ViewBag.FieldAgents = _agentsModel.GetFieldAgents();
            ViewBag.GroupLeaders = _agentsModel.GetGroupLeaders();
            ViewBag.Managers = _agentsModel.GetManagers();
        }
    }
}
