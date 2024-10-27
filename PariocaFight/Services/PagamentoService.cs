using Dapper;
using Microsoft.Data.SqlClient;
using PariocaFight.VO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PariocaFight.Services
{
    public class PagamentoService
    {
        private readonly IConfiguration _config;

        public PagamentoService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public List<string> SalvarPagamento(PagamentoVO pagamento, string nome)
        {
            var querys = new List<string>();
            var ListaErros = new List<string>();

            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                var trans = conn.BeginTransaction();

                try
                {
                    var dataFormatada = pagamento.DataPagamento.ToString("yyyy-MM-dd");

                    var selectAluno = $@"select AlunoId from Alunos
                                         where nome = '{nome}'";
                    var alunoId = conn.Query<int>(selectAluno, null, trans).FirstOrDefault();


                    var insertPagamento = $@"insert into pagamentos (alunoId, DataPagamento, Valor, Status, FormaPagamento)
                                                values ( '{alunoId}', '{dataFormatada}', '{pagamento.Valor}', '{pagamento.Status}', '{pagamento.FormaPagamento}')";

                    var resultado = conn.Execute(insertPagamento, null, trans).ToString();

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ListaErros.Add(ex.Message);
                }
            }

            return ListaErros;
        }
    }
}
