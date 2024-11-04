using Microsoft.AspNetCore.Mvc;
using PariocaFight.Data;
using PariocaFight.Repository;
using PariocaFight.Views.Visualizacao;
using PariocaFight.VO;

namespace PariocaFight.Controllers
{
    public class VisualizacaoAulaController : Controller
    {
        private readonly AulaRepository _aulaRepository;
        private readonly ApplicationDbContext _context;


        public VisualizacaoAulaController(AulaRepository aulaRepository, ApplicationDbContext context)
        {
            _aulaRepository = aulaRepository;
            _context = context;
        }

        public IActionResult Index(int numeroPagina = 1, int tamanhoPagina = 10)
        {
            if (numeroPagina == 0)
            {
                numeroPagina = 5; 
            }

            var aulas = _aulaRepository.ObterAulaPaginado(numeroPagina, tamanhoPagina);
            var totalAulas = _aulaRepository.ObterTotalAulas();

            if (totalAulas == 0)
            {
                return View("~/Views/Visualizacao/Aulas/AulaVisualizacao.cshtml", Enumerable.Empty<object>());
            }

            foreach (var obter in aulas)
            {                
                var instrutorNome = _aulaRepository.ObterNomePorInstrutorId(obter.InstrutorId);

                obter.Nome = instrutorNome;
            }

            ViewBag.TotalAulas = totalAulas;
            ViewBag.TamanhoPagina = tamanhoPagina;
            ViewBag.NumeroPagina = numeroPagina;

            return View("~/Views/Visualizacao/Aulas/AulaVisualizacao.cshtml", aulas);
        }


        public IActionResult Editar(int id)
        {
            var aula = _aulaRepository.ObeterAulaId(id);
            if (aula == null)
            {
                return NotFound();
            }

            var aulas = _aulaRepository.ObterTodasAulas();

            foreach (var obter in aulas)
            {
                var instrutorNome = _aulaRepository.ObterNomePorInstrutorId(obter.InstrutorId);

                aula.Nome = instrutorNome;
            }

            return View("~/Views/Visualizacao/Aulas/AulaEditar.cshtml", aula);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(AulasVO aula)
        {
            var instrutorId = _aulaRepository.ObterIdInstrutor(aula.Nome);

            aula.InstrutorId = instrutorId;

            
                _aulaRepository.UpdateAula(aula);
            return View("~/Views/Visualizacao/Aulas/AulaEditar.cshtml", aula);



        }

        public IActionResult Deletar(int id)
        {
            if (ModelState.IsValid)
            {
                _aulaRepository.DeletarAula(id);
                return RedirectToAction("Index");
            }

            return View("~/Views/Visualizacao/Aulas/AulaVisualizacao.cshtml", id);
        }
    }
}
