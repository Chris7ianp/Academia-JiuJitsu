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

        
        public VisualizacaoAlunosController(AlunoRepository alunoRepository, ApplicationDbContext context)
        {
            _alunoRepository = alunoRepository;
            _context = context;
        }


        public IActionResult Index(int pageNumber = 1, int pageSize = 5)
        {
            if (pageSize <= 0)
            {
                pageSize = 5;
            }

            var alunos = _alunoRepository.GetAlunosPaginated(pageNumber, pageSize);
            var totalAlunos = _alunoRepository.GetTotalAlunos();

            if (totalAlunos == 0)
            {
                return View("~/Views/Visualizacao/Alunos/AlunoVisualizacao.cshtml", Enumerable.Empty<object>());
            }

            ViewBag.TotalAlunos = totalAlunos;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;

            return View("~/Views/Visualizacao/Alunos/AlunoVisualizacao.cshtml", alunos);
        }

        public IActionResult Edit(int id)
        {
            var aluno = _alunoRepository.GetAlunoById(id);
            if (aluno == null)
            {
                return NotFound();
            }
            return View("~/Views/Visualizacao/Alunos/AlunoEditar.cshtml", aluno); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AlunosVO aluno)
        {
            if (ModelState.IsValid)
            {
                _alunoRepository.UpdateAluno(aluno); 
                return RedirectToAction("Index"); 
            }
            return View(aluno);
        }

        public IActionResult delete(int id)
        {
            if (ModelState.IsValid)
            {
                _alunoRepository.DeleteAluno(id);
                return RedirectToAction("Index");
            }
            return View("~/Views/Visualizacao/Alunos/AlunoVisualizacao.cshtml", id);
        }



    }
}
