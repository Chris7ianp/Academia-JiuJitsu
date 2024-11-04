using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;


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

        [HttpPost]
        public async Task<IActionResult> Logar(string username, string senha2, bool manterlogado)
        {
            using (SqlConnection sqlConnection = new SqlConnection("Server=DESKTOP-905VOI2\\MSSQLSERVER2022;Database=Login;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"))
            {
                await sqlConnection.OpenAsync();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = $"select * from usuarios where nomeusuario = '{username}' and senha = '{senha2}'";

                SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    int id = reader.GetInt32(0);
                    string nomeusuario = reader.GetString(1);
                    //string senha = reader.GetString(2);

                    List<Claim> diretosAcesso = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                        new Claim(ClaimTypes.Name,nomeusuario)
                    };


                    var identity = new ClaimsIdentity(diretosAcesso, "Identity.Login");

                    var userPrincipal = new ClaimsPrincipal(new[] { identity });

                    await HttpContext.SignInAsync(userPrincipal, new AuthenticationProperties
                    {
                        IsPersistent = manterlogado,
                        ExpiresUtc = DateTime.Now.AddHours(1)
                    });


                    return View("~/Views/Home/Index.cshtml"/*new { Msg = "Usuário logado com sucesso!" }*/);
                }

                return Json(new { Msg = "Usuário não encontrado! Verifique suas credenciais!" });
            }
        }

        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();

            }
            return RedirectToAction("Index", "Login");
        }
    }
}
