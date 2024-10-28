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

        public IActionResult Index()
        {
            IEnumerable<AlunosVO> alunos = _alunoRepository.GetAllAlunos();
            return View("~/Views/Visualizacao/CadastroAluno.cshtml", alunos);
        }

       
    }
}
