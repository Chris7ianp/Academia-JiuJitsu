using PariocaFight.VO;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PariocaFight.Services
{
    public class AlunoService
    {
        private readonly IConfiguration _config;

        public AlunoService(IConfiguration configuration)
        {
            _config = configuration;

        }
        public AlunosVO ObterAlunoId(string nome)
        {           

            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var query = $@"select *
                                      from Alunos
                                     where nome = '{nome}'";

                return conn.Query<AlunosVO>(query).FirstOrDefault();
            }
        }

        public List<AlunosVO> SalvarAlunos(AlunosVO alunos)
        {
            var querys = new List<AlunosVO>();

            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                var trans = conn.BeginTransaction();

                try
                {
                    var selectAlunos = $@"select *
                                            from Alunos
                                          where nome = '{alunos.Nome}'";

                    var resultado = conn.Query<AlunosVO>(selectAlunos, null, trans).ToList();

                    //querys.Add(selectAlunos);

                    if (resultado.Count() <= 0)
                    {
                        var insertAlunos = $@"INSERT INTO Alunos(nome, Sobrenome, Idade, Faixa, Contato, NomeResponsavel)
	                                                      values('{alunos.Nome}', '{alunos.Sobrenome}', '{alunos.Idade}', '{alunos.Faixa}', '{alunos.Contato}', '{alunos.NomeResponsavel}')";

                        conn.Execute(insertAlunos, null, trans);
                    }
                    else
                    {
                        var updateAlunos = $@"UPDATE alunos 
                                                SET nome = '{alunos.Nome}', 
                                                    Sobrenome = '{alunos.Sobrenome}', 
                                                    idade = '{alunos.Idade}', 
                                                    faixa = '{alunos.Faixa}', 
                                                    contato = '{alunos.Contato}', 
                                                    NomeResponsavel = '{alunos.NomeResponsavel}'
                                                WHERE nome = '{alunos.Nome}'";

                        conn.Execute(updateAlunos, null, trans);
                    }

                    trans.Commit();
                    querys = resultado;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }

                return querys;
            }
                        
        }
    }
}
