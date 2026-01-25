using AppAcademia.Data;
using AppAcademia.Filters;
using AppAcademia.Models;
using AppAcademia.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAcademia.Controllers;

[ServiceFilter(typeof(InstrutorOnlyAttribute))]
public class TreinoInstrutorController : Controller
{
    private readonly AppDbContext _context;

    public TreinoInstrutorController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var instrutorId = _context.Instrutores
            .Where(i => i.UsuarioId == HttpContext.Session.GetInt32("UsuarioId"))
            .Select(i => i.Id)
            .First();

        var alunos = _context.Alunos
            .Include(a => a.Usuario)
            .Where(a => a.InstrutorId == instrutorId)
            .ToList();

        return View(alunos);
    }

    public IActionResult Create(int alunoId)
    {
        var vm = new TreinoViewModel
        {
            AlunoId = alunoId
        };

        return View(vm);
    }

    [HttpPost]
    public IActionResult Create(TreinoViewModel model)
    {
        var treino = new Treino
        {
            AlunoId = model.AlunoId,
            DiaSemana = model.DiaSemana
        };

        _context.Treinos.Add(treino);
        _context.SaveChanges();

        foreach (var ex in model.Exercicios)
        {
            var exercicio = new Exercicio
            {
                TreinoId = treino.Id,
                Nome = ex.Nome,
                GrupoMuscular = ex.GrupoMuscular,
                Series = ex.Series,
                Repeticoes = ex.Repeticoes,
                Descanso = ex.Descanso,
                Observacoes = ex.Observacoes
            };

            _context.Exercicios.Add(exercicio);
        }

        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}
