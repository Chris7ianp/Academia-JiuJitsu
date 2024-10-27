using Microsoft.AspNetCore.Mvc;

namespace PariocaFight.Controllers
{
    public class ClienteVisualizacaoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
