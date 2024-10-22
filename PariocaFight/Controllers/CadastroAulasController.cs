using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PariocaFight.Services;
using PariocaFight.VO;
using System.Linq;


namespace PariocaFight.Controllers
{
    public class CadastroAulasController : Controller
    {
        private IConfiguration _config;

        public CadastroAulasController(IConfiguration configuration)
        {
            _config = configuration;
        }

        public ActionResult Index()
        {
            var aulasVO = new AulasVO();

            return View("~/Views/Cadastros/CadastroAulas.cshtml", aulasVO);
        }

        public ActionResult BuscarAula(string nomeAula)
        {
            var aulaService = new AulaService(_config);
            var instrutores = new InstrutorService(_config);

            var aulas = aulaService.ObterAulas(nomeAula);

            if(aulas != null)
            {
                var instrutor = instrutores.ObterInstrutores(aulas.Nome);

                aulas.Nome = instrutor.Nome;
            }

            //var instrutor = instrutores.ObterInstrutores(aulas.Nome);

            //aulas.Nome = instrutor.Nome;

            return Json(aulas);
        }


        public string SalvarAulas(AulasVO aulas)
        {
            var aulasService = new AulaService(_config);

            var aula = aulasService.SalvarAulas(aulas);

            return JsonConvert.SerializeObject(aulas);
        }

    }
}
