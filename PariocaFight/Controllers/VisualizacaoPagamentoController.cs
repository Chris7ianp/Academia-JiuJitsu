using Microsoft.AspNetCore.Mvc;
using PariocaFight.Data;
using PariocaFight.Repository;
using PariocaFight.VO;

namespace PariocaFight.Controllers
{
    public class VisualizacaoPagamentoController : Controller
    {
        private readonly PagamentoRepository _pagamentoRepository;
        private readonly ApplicationDbContext _context;


        public VisualizacaoPagamentoController(PagamentoRepository pagamentoRepository, ApplicationDbContext context)
        {
            _pagamentoRepository = pagamentoRepository;
            _context = context;
        }

        public IActionResult Index(int numeroPagina = 1, int tamanhoPagina = 10)
        {
            if (numeroPagina == 0)
            {
                numeroPagina = 10; // Corrigir o número da página se for zero
            }

            var pagamentos = _pagamentoRepository.ObterPagamentoPaginado(numeroPagina, tamanhoPagina);
            var totalPagamentos = _pagamentoRepository.ObterTotalPagamentos();

            if (totalPagamentos == 0)
            {
                return View("~/Views/Visualizacao/Pagamentos/PagamentoVisualizacao.cshtml", Enumerable.Empty<object>());
            }

            foreach (var pagamento in pagamentos)
            {
                // Obter o nome do aluno correspondente ao AlunoId
                var alunoNome = _pagamentoRepository.ObterNomePorAlunoId(pagamento.AlunoId);



                // Adicionar o nome ao objeto pagamento (assumindo que você tem uma propriedade Nome)
                pagamento.Nome = alunoNome;
            }


            
            ViewBag.TotalPagamentos = totalPagamentos;
            ViewBag.TamanhoPagina = tamanhoPagina;
            ViewBag.NumeroPagina = numeroPagina;

            return View("~/Views/Visualizacao/Pagamentos/PagamentoVisualizacao.cshtml", pagamentos);
        }


        public IActionResult Editar(int id)
        {
            var pagamento = _pagamentoRepository.ObeterPagamentoId(id);
            if (pagamento == null)
            {
                return NotFound();
            }
            return View("~/Views/Visualizacao/Pagamentos/PagamentoEditar.cshtml", pagamento);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public IActionResult Deletar(int id)
        {
            if (ModelState.IsValid)
            {
                _pagamentoRepository.DeletarPagamento(id);
                return RedirectToAction("Index");
            }

            return View("~/Views/Visualizacao/Pagamentos/PagamentoVisualizacao.cshtml", id);
        }
    }
}
