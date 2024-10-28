using Dapper;
using Microsoft.Data.SqlClient;
using PariocaFight.VO;
using System.Data;

namespace PariocaFight.Repository
{
    public class InstrutorRepository
    {
        private readonly string _connectionString;

        public InstrutorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public IEnumerable<InstrutoresVO> GetAllInstrutores()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = "Select * from Instrutores";

                dbConnection.Open();
                return dbConnection.Query<InstrutoresVO>(query).ToList();
            }
        }

        public IEnumerable<InstrutoresVO> ObterAlunosPaginado(int numeroPagina, int tamanhoPagina)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var paginacao = (numeroPagina - 1) * tamanhoPagina;

                var sql = @"SELECT * FROM Instrutores 
                                ORDER BY InstrutorId 
                                OFFSET @paginacao ROWS FETCH NEXT @tamanhoPagina ROWS ONLY";

                dbConnection.Open();
                return dbConnection.Query<InstrutoresVO>(sql, new { paginacao = paginacao, tamanhoPagina = tamanhoPagina });
            }
        }

        public int ObterTotalInstrutores()
        {
            using (IDbConnection dbConnection = Connection)
            {
                var sql = "Select Count(*) from Instrutores";

                return dbConnection.ExecuteScalar<int>(sql);
            }
        }

        public InstrutoresVO ObeterInstrutorId(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var sql = "SELECT * FROM Instrutores WHERE InstrutorId = @InstrutorId";
                dbConnection.Open();
                return dbConnection.QueryFirstOrDefault<InstrutoresVO>(sql, new { InstrutorId = id });
            }
        }


        public void UpdateInstrutor(InstrutoresVO instrutor)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var sql = @"update instrutores 
				               set nome = @Nome,
				               sobrenome = @SobreNome,
				               faixa = @Faixa,
				               ArteMarcial = @ArteMarcial
		                     where instrutorId = @InstrutorId";

                dbConnection.Open();
                dbConnection.Execute(sql, instrutor);
            }
        }

        public void DeletarInstrutor(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var sql = @"DELETE FROM Instrutores WHERE InstrutorId = @InstrutorId";


                dbConnection.Open();
                dbConnection.Execute(sql, new { InstrutorId = id });
            }
        }
    }
}
