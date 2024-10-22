using Dapper;
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

                    if (resultado.Count() == 0)
                    {
                        var instrutor = "";

                        foreach(var item in resultado)
                        {
                            instrutor = item.Nome;
                        }


                        var insertAula = $@"insert into Aulas (DiaSemana,                    NomeAula,           horario)
                                                       values ('{aulas.DiaSemana}',  '{aulas.NomeAula}', '{aulas.HorarioFormatado}' )";

                        querys.Add(insertAula);
                    }
                    else
                    {
                        var updateAulas = $@"";

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
