using Dapper;
using FluentAssertions.Equivalency;
using Microsoft.Data.SqlClient;
using PariocaFight.VO;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace PariocaFight.Repository
{
    public class PagamentoRepository
    {
        private readonly string _connectionString;

        public PagamentoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public IEnumerable<PagamentoVO> ObterTodosPagamentos()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = "Select * from Pagamentos";

                dbConnection.Open();
                return dbConnection.Query<PagamentoVO>(query).ToList();
            }
        }

        public IEnumerable<PagamentoVO> ObterPagamentoPaginado(int numeroPagina, int tamanhoPagina)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var paginacao = (numeroPagina - 1) * tamanhoPagina;


                var sql = @"SELECT * FROM Pagamentos 
                                ORDER BY PagamentoId 
                                OFFSET @paginacao ROWS FETCH NEXT @tamanhoPagina ROWS ONLY";

                dbConnection.Open();
                return dbConnection.Query<PagamentoVO>(sql, new { paginacao = paginacao, tamanhoPagina = tamanhoPagina });
            }
        }

        public int ObterTotalPagamentos()
        {
            using (IDbConnection dbConnection = Connection)
            {
                var sql = "Select Count(*) from Pagamentos";

                return dbConnection.ExecuteScalar<int>(sql);
            }
        }

        public PagamentoVO ObeterPagamentoId(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var sql = "SELECT * FROM Pagamentos WHERE PagamentoId = @PagamentoId";
                dbConnection.Open();
                return dbConnection.QueryFirstOrDefault<PagamentoVO>(sql, new { PagamentoId = id });
            }
        }

        public void DeletarPagamento(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var sql = @"DELETE FROM Pagamentos WHERE PagamentoId = @PagamentoId";


                dbConnection.Open();
                dbConnection.Execute(sql, new { PagamentoId = id });
            }
        }

        public string ObterNomePorAlunoId(int alunoId)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sqlQuery = "SELECT Nome FROM alunos WHERE AlunoId = @AlunoId";

                // Executa a query e retorna o nome correspondente ou null se não encontrado
                return dbConnection.QuerySingleOrDefault<string>(sqlQuery, new { AlunoId = alunoId });
            }
        }

    }
}
