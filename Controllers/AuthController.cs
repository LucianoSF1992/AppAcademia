using AppAcademia.Data;
using AppAcademia.Helpers;
using AppAcademia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppAcademia.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // ===============================
        // LOGIN (GET)
        // ===============================
        public IActionResult Login()
        {
            return View();
        }

        // ===============================
        // LOGIN (POST)
        // ===============================
        [HttpPost]
        public IActionResult Login(string email, string senha)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null || !PasswordHelper.Verificar(senha, usuario.Senha))
            {
                ViewBag.Erro = "E-mail ou senha inválidos.";
                return View();
            }

            // ===============================
            // GERAR JWT
            // ===============================
            var token = GerarToken(usuario);

            // ===============================
            // SESSION (opcional – útil para MVC)
            // ===============================
            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("Perfil", usuario.Perfil);
            HttpContext.Session.SetString("JwtToken", token);

            // ===============================
            // REDIRECIONAR POR PERFIL
            // ===============================
            return usuario.Perfil switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "Instrutor" => RedirectToAction("Dashboard", "Instrutor"),
                "Aluno" => RedirectToAction("Index", "Aluno"),
                _ => RedirectToAction("Login")
            };
        }

        // ===============================
        // LOGOUT
        // ===============================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ===============================
        // MÉTODO PRIVADO → GERAR TOKEN JWT
        // ===============================
        private string GerarToken(Usuario usuario)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.Perfil)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(jwtSettings["ExpireMinutes"])
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
