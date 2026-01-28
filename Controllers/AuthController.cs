using AppAcademia.Data;
using Microsoft.AspNetCore.Mvc;
using AppAcademia.Helpers;

namespace AppAcademia.Controllers;

public class AuthController : Controller
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string senha)
    {
        var usuario = _context.Usuarios
            .FirstOrDefault(u => u.Email == email);

        if (usuario == null || !PasswordHelper.Verificar(senha, usuario.Senha))
        {
            ViewBag.Erro = "Email ou senha inválidos";
            return View();
        }

        HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
        HttpContext.Session.SetString("Nome", usuario.Nome);
        HttpContext.Session.SetString("Perfil", usuario.Perfil);

        return usuario.Perfil switch
        {
            "Admin" => RedirectToAction("Dashboard", "Admin"),
            "Instrutor" => RedirectToAction("Dashboard", "Instrutor"),
            "Aluno" => RedirectToAction("Index", "Aluno"),
            _ => RedirectToAction("Login")
        };
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
