using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PariocaFight.Services;
using PariocaFight.VO;



namespace PariocaFight.Controllers
{
    public class CadastroAlunoController : Controller
    {
        private IConfiguration _config;
        public CadastroAlunoController(IConfiguration configuration)
        {
            _config = configuration;
        }

        public ActionResult Index()
        {
            return View("~/Views/Cadastros/CadastroAluno.cshtml");
        }

        public ActionResult BuscarAluno(string nome)
        {
            var alunoService = new AlunoService(_config);

            var aluno = alunoService.ObterAlunoId(nome);

            if (aluno == null)
            {
                return Json(new { });
            }

            return Json(aluno);
        }

        public string SalvarAluno(AlunosVO alunos)
        {
            var alunoService = new AlunoService(_config);
            
            var aluno = alunoService.SalvarAlunos(alunos).ToList();

            return JsonConvert.SerializeObject(aluno);
        }

    }
}

