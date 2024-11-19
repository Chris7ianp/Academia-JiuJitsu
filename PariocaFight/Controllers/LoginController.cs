using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;


namespace Login.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login

     
        public ActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                return Json(new { Msg = "Usuario já logado." });
            }

            return View();
        }


        //public IActionResult Logar()
        //{
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> Logar(string username, string senha2, bool manterlogado)
        {
            // String de conexão
            string connectionString = "Server=DESKTOP-905VOI2\\MSSQLSERVER2022;Database=Login;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

            // Abrindo conexão com o banco
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                // Consulta ao banco de dados
                string query = "SELECT Id, NomeUsuario FROM Usuarios WHERE NomeUsuario = @Username AND Senha = @Senha";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Username", username);
                    sqlCommand.Parameters.AddWithValue("@Senha", senha2);

                    SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                    // Verifica se o usuário foi encontrado
                    if (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32(0); // Id do usuário
                        string nomeUsuario = reader.GetString(1); // Nome do usuário

                        // Criando os claims (informações sobre o usuário)
                        var diretosAcesso = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                            new Claim(ClaimTypes.Name, nomeUsuario)
                        };

                                // Criando a identidade e o principal
                        var identity = new ClaimsIdentity(diretosAcesso, CookieAuthenticationDefaults.AuthenticationScheme);
                        var userPrincipal = new ClaimsPrincipal(identity);

                        // Configurando as propriedades de autenticação
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = manterlogado, // Mantém login após fechar o navegador
                            ExpiresUtc = DateTime.UtcNow.AddHours(1) // Expira em 1 hora
                        };

                        // Realiza o login
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);

                        // Redireciona para a página inicial
                        return RedirectToAction("Index", "Home");
                    }

                    // Usuário não encontrado
                    return Json(new { Msg = "Usuário não encontrado! Verifique suas credenciais!" });
                }
            }
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
