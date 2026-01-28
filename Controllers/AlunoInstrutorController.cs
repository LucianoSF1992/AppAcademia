using AppAcademia.Data;
using AppAcademia.Filters;
using AppAcademia.Models;
using AppAcademia.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAcademia.Controllers;
[AuthorizeSession("Instrutor")]
public class AlunoInstrutorController : Controller
{
    private readonly AppDbContext _context;

    public AlunoInstrutorController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var instrutorId = _context.Instrutores
            .Where(i => i.UsuarioId == HttpContext.Session.GetInt32("UsuarioId"))
            .Select(i => i.Id)
            .FirstOrDefault();

        var alunos = _context.Alunos
            .Include(a => a.Usuario)
            .Where(a => a.InstrutorId == instrutorId)
            .ToList();

        return View(alunos);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(AlunoViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var instrutorId = _context.Instrutores
            .Where(i => i.UsuarioId == HttpContext.Session.GetInt32("UsuarioId"))
            .Select(i => i.Id)
            .First();

        var usuario = new Usuario
        {
            Nome = model.Nome,
            Email = model.Email,
            Senha = model.Senha,
            Perfil = "Aluno"
        };

        _context.Usuarios.Add(usuario);
        _context.SaveChanges();

        var aluno = new Aluno
        {
            UsuarioId = usuario.Id,
            InstrutorId = instrutorId,
            Idade = model.Idade,
            Objetivo = model.Objetivo
        };

        _context.Alunos.Add(aluno);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var aluno = _context.Alunos
            .Include(a => a.Usuario)
            .FirstOrDefault(a => a.Id == id);

        if (aluno == null)
            return NotFound();

        var vm = new AlunoViewModel
        {
            Id = aluno.Id,
            Nome = aluno.Usuario!.Nome,
            Email = aluno.Usuario.Email,
            Idade = aluno.Idade,
            Objetivo = aluno.Objetivo
        };

        return View(vm);
    }

    [HttpPost]
    public IActionResult Edit(AlunoViewModel model)
    {
        var aluno = _context.Alunos
            .Include(a => a.Usuario)
            .FirstOrDefault(a => a.Id == model.Id);

        if (aluno == null)
            return NotFound();

        aluno.Usuario!.Nome = model.Nome;
        aluno.Usuario.Email = model.Email;

        if (!string.IsNullOrEmpty(model.Senha))
            aluno.Usuario.Senha = model.Senha;

        aluno.Idade = model.Idade;
        aluno.Objetivo = model.Objetivo;

        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var aluno = _context.Alunos
            .Include(a => a.Usuario)
            .FirstOrDefault(a => a.Id == id);

        if (aluno == null)
            return NotFound();

        // 1️⃣ remove Aluno
        _context.Alunos.Remove(aluno);

        // 2️⃣ remove Usuario
        _context.Usuarios.Remove(aluno.Usuario!);

        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}
