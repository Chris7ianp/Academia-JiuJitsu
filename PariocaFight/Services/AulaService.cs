using Dapper;
using Humanizer;
using Microsoft.Data.SqlClient;
using PariocaFight.VO;

namespace PariocaFight.Services
{
    public class AulaService
    {
        private readonly IConfiguration _config;

        public AulaService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public AulasVO ObterAulas(string nomeAula)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var selectAula = $@"SELECT 
                                         A.aulaid, 
                                         CONVERT(VARCHAR(5), A.Horario, 108) AS HorarioFormatado, 
                                         A.diasemana, 
                                         A.InstrutorId, 
                                         A.NomeAula,
                                         I.Nome AS Nome
                                      FROM 
                                         Aulas A
                                      JOIN 
                                         Instrutores I ON A.InstrutorId = I.InstrutorId

                                where NomeAula = '{nomeAula}'";

                return conn.Query<AulasVO>(selectAula).FirstOrDefault();
            }
        }

        public List<AulasVO> BuscarInstrutor()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var selectInstrutor = $@"select nome, InstrutorId from Instrutores";

                var result = conn.Query<AulasVO>(selectInstrutor).ToList();

                return result;
            }

        }


        public List<string> SalvarAulas(AulasVO aulas)
        {
            var querys = new List<string>();
            var ListaErros = new List<string>();

            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                var trans = conn.BeginTransaction();

                try
                {
                    var selectAula = $@"SELECT 
                                         A.aulaid, 
                                         CONVERT(VARCHAR(5), A.Horario, 108) AS HorarioFormatado, 
                                         A.diasemana, 
                                         A.InstrutorId, 
                                         A.NomeAula,
                                         I.Nome AS Nome
                                      FROM 
                                         Aulas A
                                      JOIN 
                                         Instrutores I ON A.InstrutorId = I.InstrutorId

                                where NomeAula = '{aulas.NomeAula}'";

                    var resultado = conn.Query<AulasVO>(selectAula, null, trans).ToList();

                    var instId = $@"select InstrutorId from Instrutores where nome = '{aulas.Nome}'";

                    var idInstrutor = conn.Query<int>(instId, null, trans).FirstOrDefault();

                    if (resultado.Count() == 0)
                    {
                        //var instId = $@"select InstrutorId from Instrutores where nome = '{aulas.Nome}'";

                        //var idInstrutor = conn.Query<int>(instId, null, trans).FirstOrDefault();


                        var insertAula = $@"insert into Aulas (nomeAula, horario, diaSemana, instrutorId)
                                                       values ('{aulas.NomeAula}', '{aulas.HorarioFormatado}', '{aulas.DiaSemana}', '{idInstrutor}')";

                        querys.Add(insertAula);
                    }
                    else
                    {
                        var updateAulas = $@"update aulas set horario = '{aulas.HorarioFormatado}',
												 nomeAula = '{aulas.NomeAula}',
												 diaSemana = '{aulas.DiaSemana}',
												 instrutorId = '{idInstrutor}'
											where nomeAula = '{aulas.NomeAula}'";

                        querys.Add(updateAulas);
                    }

                    if (ListaErros.Count == 0)
                    {
                        querys.ForEach(query => conn.Execute(query, null, trans));
                    }
                    else
                    {
                        trans.Rollback();
                        return ListaErros;
                    }


                    trans.Commit();


                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ListaErros.Add(ex.Message);
                }

                return ListaErros;
            }
        }
    }
}
