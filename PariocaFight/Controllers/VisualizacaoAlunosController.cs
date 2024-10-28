using Microsoft.AspNetCore.Mvc;
using PariocaFight.Data;
using PariocaFight.Repository;
using PariocaFight.VO;

namespace PariocaFight.Controllers
{
    public class VisualizacaoAlunosController : Controller
    {
        private readonly AlunoRepository _alunoRepository;
        private readonly ApplicationDbContext _context;

        // Combine os dois construtores em um só
        public VisualizacaoAlunosController(AlunoRepository alunoRepository, ApplicationDbContext context)
        {
            _alunoRepository = alunoRepository;
            _context = context;
        }

        //public IActionResult Index()
        //{
        //    IEnumerable<AlunosVO> alunos = _alunoRepository.GetAllAlunos();

        //    return View("~/Views/Visualizacao/CadastroAluno.cshtml", alunos);
        //}

        public IActionResult Index(int pageNumber = 1, int pageSize = 3)
        {
            if (pageSize <= 0)
            {
                pageSize = 3;
            }

            var alunos = _alunoRepository.GetAlunosPaginated(pageNumber, pageSize);
            var totalAlunos = _alunoRepository.GetTotalAlunos();

            if (totalAlunos == 0)
            {
                return View("~/Views/Visualizacao/AlunoVisualizacao.cshtml", Enumerable.Empty<object>());
            }

            ViewBag.TotalAlunos = totalAlunos;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;

            return View("~/Views/Visualizacao/AlunoVisualizacao.cshtml", alunos);
        }



    }
}
