using Microsoft.AspNetCore.Mvc;
using PariocaFight.Data;
using PariocaFight.Repository;
using PariocaFight.VO;

namespace PariocaFight.Controllers
{
    public class VisualizacaoInstrutorController : Controller
    {
        private readonly InstrutorRepository _instrutorRepository;
        private readonly ApplicationDbContext _context;


        public VisualizacaoInstrutorController(InstrutorRepository alunoRepository, ApplicationDbContext context)
        {
            _instrutorRepository = alunoRepository;
            _context = context;
        }

        public IActionResult Index(int numeroPagina = 1, int tamanhoPagina = 10)
        {
            if (numeroPagina == 0)
            {
                tamanhoPagina = 10;
            }

            var instrutores = _instrutorRepository.ObterAlunosPaginado(numeroPagina, tamanhoPagina);
            var totalIntrutores = _instrutorRepository.ObterTotalInstrutores();

            if (totalIntrutores == 0)
            {
                return View("~/Views/Visualizacao/Instrutores/InstrutorVisualizacao.cshtml", Enumerable.Empty<object>());
            }

            ViewBag.TotalInstrutores = totalIntrutores;
            ViewBag.tamanhoPagina = tamanhoPagina;
            ViewBag.numeroPagina = numeroPagina;

            return View("~/Views/Visualizacao/Instrutores/InstrutorVisualizacao.cshtml", instrutores);
        }

        public IActionResult Editar(int id)
        {
            var instrutor = _instrutorRepository.ObeterInstrutorId(id);
            if (instrutor == null)
            {
                return NotFound();
            }
            return View("~/Views/Visualizacao/Instrutores/InstrutorEditar.cshtml", instrutor);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(InstrutoresVO instrutor) 
        {
            if (ModelState.IsValid)
            {
                _instrutorRepository.UpdateInstrutor(instrutor);
                return RedirectToAction("Index");
            }

            return View(instrutor);
        }

        public IActionResult Deletar(int id)
        {
            if (ModelState.IsValid)
            {
                _instrutorRepository.DeletarInstrutor(id);
                return RedirectToAction("Index");
            }

            return View("~/Views/Visualizacao/Instrutores/InstrutorVisualizacao.cshtml", id);
        }
    }
}
