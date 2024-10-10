using Microsoft.AspNetCore.Mvc;

namespace PariocaFight.Controllers
{
    public class CadastroInstrutoresController : Controller
    {
        private IConfiguration _config;
        public CadastroInstrutoresController(IConfiguration configuration)
        {
            _config = configuration;
        }

        public ActionResult Index()
        {
            return View("~/Views/Cadastros/CadastroInstrutores.cshtml");
        }

    }
}
