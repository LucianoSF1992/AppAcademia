using AppAcademia.Data;
using AppAcademia.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppAcademia.ViewModels;
using AppAcademia.Models;

namespace AppAcademia.Controllers;

[ServiceFilter(typeof(AlunoOnlyAttribute))]
public class AlunoController : Controller
{
    private readonly AppDbContext _context;

    public AlunoController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

        var alunoId = _context.Alunos
            .Where(a => a.UsuarioId == usuarioId)
            .Select(a => a.Id)
            .First();

        var diaHoje = DateTime.Now.DayOfWeek switch
        {
            DayOfWeek.Monday => "Segunda",
            DayOfWeek.Tuesday => "Terça",
            DayOfWeek.Wednesday => "Quarta",
            DayOfWeek.Thursday => "Quinta",
            DayOfWeek.Friday => "Sexta",
            DayOfWeek.Saturday => "Sábado",
            _ => "Domingo"
        };


        var treinos = _context.Treinos
            .Include(t => t.Exercicios)
            .Where(t => t.AlunoId == alunoId && t.DiaSemana == diaHoje)
            .ToList();

        var concluidosHoje = _context.ExerciciosConcluidos
            .Where(c => c.DataConclusao.Date == DateTime.Today)
            .Select(c => c.ExercicioId)
            .ToList();

        ViewBag.Concluidos = concluidosHoje;

        return View(treinos);
    }

    [HttpPost]
    public IActionResult ConcluirExercicio(int exercicioId)
    {
        var existe = _context.ExerciciosConcluidos
            .Any(c => c.ExercicioId == exercicioId && c.DataConclusao.Date == DateTime.Today);

        if (!existe)
        {
            _context.ExerciciosConcluidos.Add(new()
            {
                ExercicioId = exercicioId,
                DataConclusao = DateTime.Now
            });

            _context.SaveChanges();
        }

        return Ok();
    }

    public IActionResult Historico()
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

        var alunoId = _context.Alunos
            .Where(a => a.UsuarioId == usuarioId)
            .Select(a => a.Id)
            .First();

        var inicioSemana = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
        var fimSemana = inicioSemana.AddDays(7);

        var historico = _context.ExerciciosConcluidos
            .Include(c => c.Exercicio!)
                .ThenInclude(e => e.Treino!)
            .Where(c =>
                c.Exercicio != null &&
                c.Exercicio.Treino != null &&
                c.Exercicio.Treino.AlunoId == alunoId &&
                c.DataConclusao >= inicioSemana &&
                c.DataConclusao < fimSemana
            )
            .GroupBy(c => c.DataConclusao.Date)
            .Select(g => new HistoricoSemanalViewModel
            {
                Data = g.Key,
                TotalExercicios = g.Count()
            })
            .OrderBy(h => h.Data)
            .ToList();

        ViewBag.InicioSemana = inicioSemana;
        ViewBag.FimSemana = fimSemana.AddDays(-1);

        return View(historico);
    }

}
