using AppAcademia.Data;
using AppAcademia.Filters;
using AppAcademia.Models;
using AppAcademia.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAcademia.Controllers;

[AuthorizeSession("Admin")]

[NoCache]
public class InstrutorAdminController : Controller
{
    private readonly AppDbContext _context;

    public InstrutorAdminController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var instrutores = _context.Instrutores
            .Include(i => i.Usuario)
            .ToList();

        return View(instrutores);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(InstrutorViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var usuario = new Usuario
        {
            Nome = model.Nome,
            Email = model.Email,
            Senha = PasswordHelper.Hash(model.Senha),
            Perfil = "Instrutor"
        };

        _context.Usuarios.Add(usuario);
        _context.SaveChanges();

        var instrutor = new Instrutor
        {
            UsuarioId = usuario.Id
        };

        _context.Instrutores.Add(instrutor);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var instrutor = _context.Instrutores
            .Include(i => i.Usuario)
            .FirstOrDefault(i => i.Id == id);

        if (instrutor == null)
            return NotFound();

        var vm = new InstrutorViewModel
        {
            Id = instrutor.Id,
            Nome = instrutor.Usuario!.Nome,
            Email = instrutor.Usuario.Email
        };

        return View(vm);
    }

    [HttpPost]
    public IActionResult Edit(InstrutorViewModel model)
    {
        var instrutor = _context.Instrutores
            .Include(i => i.Usuario)
            .FirstOrDefault(i => i.Id == model.Id);

        if (instrutor == null)
            return NotFound();

        instrutor.Usuario!.Nome = model.Nome;
        instrutor.Usuario.Email = model.Email;

        if (!string.IsNullOrEmpty(model.Senha))
            instrutor.Usuario.Senha = model.Senha;

        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var instrutor = _context.Instrutores
            .Include(i => i.Alunos)
            .FirstOrDefault(i => i.Id == id);

        if (instrutor == null)
            return NotFound();

        if (instrutor.Alunos.Any())
        {
            TempData["Erro"] = "Não é possível excluir este instrutor pois existem alunos vinculados a ele.";
            return RedirectToAction("Index");
        }

        _context.Instrutores.Remove(instrutor);
        _context.SaveChanges();

        TempData["Sucesso"] = "Instrutor excluído com sucesso.";
        return RedirectToAction("Index");
    }
}
