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
                Observacoes = ex.Observacoes ?? ""
            };

            _context.Exercicios.Add(exercicio);
        }

        _context.SaveChanges();

        return RedirectToAction("Index");
    }
    public IActionResult Detalhes(int alunoId)
    {
        var aluno = _context.Alunos
            .Include(a => a.Usuario)
            .FirstOrDefault(a => a.Id == alunoId);

        if (aluno == null)
            return NotFound();

        var treinos = _context.Treinos
            .Where(t => t.AlunoId == alunoId && t.Ativo)
            .Select(t => new TreinoStatusViewModel
            {
                TreinoId = t.Id,
                DiaSemana = t.DiaSemana,
                TotalExercicios = t.Exercicios.Count(),

                ExerciciosConcluidos = _context.ExerciciosConcluidos
                .Count(c => c.Exercicio!.TreinoId == t.Id)
            })
            .ToList();


        ViewBag.Aluno = aluno;
        return View(treinos);
    }

    public IActionResult DetalhesTreino(int id)
    {
        var treino = _context.Treinos
            .Include(t => t.Exercicios)
            .FirstOrDefault(t => t.Id == id);

        if (treino == null)
            return NotFound();

        return View(treino);
    }

    public IActionResult Delete(int id)
    {
        var treino = _context.Treinos
            .FirstOrDefault(t => t.Id == id);

        if (treino == null)
            return NotFound();

        bool jaIniciado = _context.ExerciciosConcluidos
            .Any(c => c.Exercicio != null && c.Exercicio.TreinoId == treino.Id);

        if (jaIniciado)
        {
            TempData["Erro"] = "Este treino não pode ser excluído pois já foi iniciado.";
            return RedirectToAction("Detalhes", new { alunoId = treino.AlunoId });
        }

        treino.Ativo = false; // SOFT DELETE
        _context.SaveChanges();

        return RedirectToAction("Detalhes", new { alunoId = treino.AlunoId });
    }
    private bool TreinoJaIniciado(int treinoId)
    {
        return _context.ExerciciosConcluidos
            .Any(c => c.Exercicio!.TreinoId == treinoId);
    }

    public IActionResult Editar(int id)
    {
        var treino = _context.Treinos
            .Include(t => t.Exercicios)
            .FirstOrDefault(t => t.Id == id);

        if (treino == null)
            return NotFound();

        if (TreinoJaIniciado(treino.Id))
        {
            TempData["Erro"] = "Este treino já foi iniciado e não pode ser editado.";
            return RedirectToAction("Detalhes", new { alunoId = treino.AlunoId });
        }

        return View(treino);
    }

    [HttpPost]
    public IActionResult Editar(Treino treino)
    {
        if (TreinoJaIniciado(treino.Id))
        {
            TempData["Erro"] = "Este treino já foi iniciado e não pode ser editado.";
            return RedirectToAction("Detalhes", new { alunoId = treino.AlunoId });
        }

        _context.Treinos.Update(treino);
        _context.SaveChanges();

        return RedirectToAction("Detalhes", new { alunoId = treino.AlunoId });
    }
}
