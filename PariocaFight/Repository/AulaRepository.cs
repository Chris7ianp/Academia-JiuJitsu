using Dapper;
using FluentAssertions.Equivalency;
using Microsoft.Data.SqlClient;
using PariocaFight.VO;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace PariocaFight.Repository
{
    public class AulaRepository
    {
        private readonly string _connectionString;

        public AulaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public IEnumerable<AulasVO> ObterTodasAulas()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = "Select * from Aulas";

                dbConnection.Open();
                return dbConnection.Query<AulasVO>(query).ToList();
            }
        }

        public IEnumerable<AulasVO> ObterAulaPaginado(int numeroPagina, int tamanhoPagina)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var paginacao = (numeroPagina - 1) * tamanhoPagina;


                var sql = @"SELECT * FROM Aulas 
                                ORDER BY AulaId 
                                OFFSET @paginacao ROWS FETCH NEXT @tamanhoPagina ROWS ONLY";

                dbConnection.Open();
                return dbConnection.Query<AulasVO>(sql, new { paginacao = paginacao, tamanhoPagina = tamanhoPagina });
            }
        }

        public int ObterTotalAulas()
        {
            using (IDbConnection dbConnection = Connection)
            {
                var sql = "Select Count(*) from Aulas";

                return dbConnection.ExecuteScalar<int>(sql);
            }
        }

        public AulasVO ObeterAulaId(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var sql = "SELECT * FROM Aulas WHERE AulaId = @AulaId";
                dbConnection.Open();
                return dbConnection.QueryFirstOrDefault<AulasVO>(sql, new { AulaId = id });
            }
        }

        public void DeletarAula(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var sql = @"DELETE FROM Aulas WHERE AulaId = @AulaId";


                dbConnection.Open();
                dbConnection.Execute(sql, new { AulaId = id });
            }
        }

        public string ObterNomePorInstrutorId(int intrutorId)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sqlQuery = "SELECT Nome FROM Instrutores WHERE InstrutorId = @InstrutorId";

               
                return dbConnection.QuerySingleOrDefault<string>(sqlQuery, new { InstrutorId = intrutorId });
            }
        }

        public void UpdateAula(AulasVO aula)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var sql = @"UPDATE aulas 
                                SET horario = @Horario,
                                    nomeAula = @NomeAula,
                                    diaSemana = @DiaSemana,
                                    instrutorId = @InstrutorId
                                WHERE AulaId = @AulaId";


                dbConnection.Open();
                dbConnection.Execute(sql, aula);
            }
        }

        public int ObterIdInstrutor(string nome)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sqlQuery = "SELECT instrutorId FROM Instrutores WHERE Nome = @Nome";

                var instrutorId = dbConnection.QuerySingleOrDefault<int>(sqlQuery, new { Nome = nome });

                return instrutorId;
            }
        }

    }


}
