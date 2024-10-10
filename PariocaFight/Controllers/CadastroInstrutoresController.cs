using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PariocaFight.Services;
using PariocaFight.VO;

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

        public ActionResult BuscarInstrutor(string nome)
            {
                var instrutorService = new InstrutorService(_config);

            var instrutor = instrutorService.ObterInstrutores(nome);

            //if (instrutor == null)
            //{
            //    return Json(new { });
            //}

            return Json(instrutor);
        }

        public string SalvarInstrutores(InstrutoresVO instrutores)
        {
            var instrutorService = new InstrutorService(_config);

            var instrutor = instrutorService.SalvarInstrutores(instrutores);

            return JsonConvert.SerializeObject(instrutor);
        }

    }
}
