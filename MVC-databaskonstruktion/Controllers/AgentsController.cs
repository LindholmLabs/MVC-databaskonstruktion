using Microsoft.AspNetCore.Mvc;

namespace MVC_databaskonstruktion.Controllers
{
    public class AgentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
