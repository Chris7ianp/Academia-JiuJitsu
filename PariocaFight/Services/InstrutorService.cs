using Dapper;
using Microsoft.Data.SqlClient;
using PariocaFight.VO;

namespace PariocaFight.Services
{
    public class InstrutorService
    {
        private readonly IConfiguration _config;

        public InstrutorService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public InstrutoresVO ObterInstrutores(string nome)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {

                var query = $@"select * from Instrutores
                                where nome = '{nome}'";

                    return conn.Query<InstrutoresVO>(query).FirstOrDefault();

            }
        }

        public List<string> SalvarInstrutores(InstrutoresVO instrutores)
        {
            var querys = new List<string>();
            var ListaErros = new List<string>();

            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                var trans = conn.BeginTransaction();

                try
                {
                    var selectInstrutores = $@"select * from instrutores
                                                where nome = '{instrutores.Nome}'";

                    var resultado = conn.Query<InstrutoresVO>(selectInstrutores, null, trans).ToList();


                    if(resultado.Count() <= 0)
                    {
                        var insertInstrutores = $@"insert into Instrutores(nome, sobrenome, faixa, ArteMarcial)
                                                                    values('{instrutores.Nome}','{instrutores.Sobrenome}','{instrutores.Faixa}','{instrutores.ArteMarcial}')";

                        querys.Add(insertInstrutores);
                    }
                    else
                    {
                        var updateInstrutores = $@"update instrutores 
				                                            set nome = '{instrutores.Nome}',
				                                            sobrenome = '{instrutores.Sobrenome}',
				                                            faixa = '{instrutores.Faixa}',
				                                            ArteMarcial = '{instrutores.ArteMarcial}'
		                                            where nome = '{instrutores.Nome}'";

                        querys.Add(updateInstrutores);
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
            }

            return ListaErros;
        } 
    }
}
