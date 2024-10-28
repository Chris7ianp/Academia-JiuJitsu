using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PariocaFight.Data;
using PariocaFight.VO;
using System.Data;
using System.Data.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public IEnumerable<AlunosVO> GetAlunosPaginated(int pageNumber, int pageSize)
        {
            using (IDbConnection dbconnection = Connection)
            {
                var offset = (pageNumber - 1) * pageSize;
                var sql = @"SELECT * FROM Alunos 
                    ORDER BY AlunoId 
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY"
                ;

                dbconnection.Open();
                return dbconnection.Query<AlunosVO>(sql, new { Offset = offset, PageSize = pageSize });
            }
        }  
        
        public int GetTotalAlunos()
        {
            using (IDbConnection dbconnection = Connection)
            {
                var sql = "SELECT COUNT(*) FROM Alunos";

                return dbconnection.ExecuteScalar<int>(sql);

            }
        }
    }
}
