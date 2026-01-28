using Microsoft.AspNetCore.Mvc;
using AppAcademia.Filters;
using AppAcademia.Data;
using Microsoft.AspNetCore.Authorization;

namespace AppAcademia.Controllers;

[ServiceFilter(typeof(AdminOnlyAttribute))]

[AuthorizeSession]
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return RedirectToAction("Dashboard");
    }


    public IActionResult Dashboard()
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        var perfil = HttpContext.Session.GetString("Perfil");

        if (usuarioId == null || perfil != "Admin")
            return RedirectToAction("Login", "Auth");

        var totalInstrutores = _context.Instrutores.Count();
        var totalAlunos = _context.Alunos.Count();
        var totalTreinos = _context.Treinos.Count(t => t.Ativo);
        var totalExercicios = _context.Exercicios.Count();

        ViewBag.TotalInstrutores = totalInstrutores;
        ViewBag.TotalAlunos = totalAlunos;
        ViewBag.TotalTreinos = totalTreinos;
        ViewBag.TotalExercicios = totalExercicios;

        return View();
    }
}

