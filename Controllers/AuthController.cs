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
            // SESSION (opcional – útil para MVC)
            // ===============================
            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("Nome", usuario.Nome);
            HttpContext.Session.SetString("Perfil", usuario.Perfil);

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
            // Limpa toda a sessão do usuário
            HttpContext.Session.Clear();

            // Mensagem amigável para o usuário
            TempData["Mensagem"] = "Você saiu do sistema com segurança.";

            // Redireciona para a tela de login
            return RedirectToAction("Login", "Auth");
        }

    }
}
