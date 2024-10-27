using Microsoft.AspNetCore.Mvc;

namespace PariocaFight.Controllers
{
    public class CadastroPagamentosController : Controller
    {
        private IConfiguration _config;
        public CadastroPagamentosController(IConfiguration configuration)
        {
            _config = configuration;
        }

        public ActionResult Index()
        {
            return View("~/Views/Cadastros/CadastroPagamento.cshtml");
        }
    }
}
