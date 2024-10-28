using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PariocaFight.Data;
using PariocaFight.VO;
using System.Data;
using System.Data.Common;

namespace PariocaFight.Repository
{
    public class AlunoRepository
    {
        private readonly string _connectionString;

        public AlunoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public IEnumerable<AlunosVO> GetAllAlunos()
        {
            using (IDbConnection dbconnection = Connection)
            {
                string query = "Select * from alunos";

                dbconnection.Open();
                return dbconnection.Query<AlunosVO>(query).ToList();
            }
        }
    }
}
