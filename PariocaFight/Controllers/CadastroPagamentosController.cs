using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PariocaFight.Services;
using PariocaFight.VO;

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

        public string SalvarPagamento(PagamentoVO pagamento, string nome)
        {
            var pagamentoService = new PagamentoService(_config);

            var result = pagamentoService.SalvarPagamento(pagamento,nome);



            return JsonConvert.SerializeObject(result);
        }
    }
}
