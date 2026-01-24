using AppAcademia.Data;
using Microsoft.AspNetCore.Mvc;

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
            .FirstOrDefault(u => u.Email == email && u.Senha == senha);

        if (usuario == null)
        {
            ViewBag.Erro = "E-mail ou senha inválidos";
            return View();
        }

        HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
        HttpContext.Session.SetString("Perfil", usuario.Perfil);
        HttpContext.Session.SetString("Nome", usuario.Nome);

        return usuario.Perfil switch
        {
            "Admin" => RedirectToAction("Index", "Admin"),
            "Instrutor" => RedirectToAction("Index", "Instrutor"),
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
