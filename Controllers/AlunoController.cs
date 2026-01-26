using AppAcademia.Data;
using AppAcademia.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppAcademia.ViewModels;

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
            .Where(c => c.Data.Date == DateTime.Today)
            .Select(c => c.ExercicioId)
            .ToList();

        ViewBag.Concluidos = concluidosHoje;

        return View(treinos);
    }

    [HttpPost]
    public IActionResult ConcluirExercicio(int exercicioId)
    {
        var existe = _context.ExerciciosConcluidos
            .Any(c => c.ExercicioId == exercicioId && c.Data.Date == DateTime.Today);

        if (!existe)
        {
            _context.ExerciciosConcluidos.Add(new()
            {
                ExercicioId = exercicioId,
                Data = DateTime.Now
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

        var inicioSemana = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1);
        var fimSemana = inicioSemana.AddDays(6);

        var treinos = _context.Treinos
            .Include(t => t.Exercicios)
            .Where(t => t.AlunoId == alunoId)
            .ToList();

        var concluidosSemana = _context.ExerciciosConcluidos
            .Where(c => c.Data.Date >= inicioSemana && c.Data.Date <= fimSemana)
            .ToList();

        ViewBag.ConcluidosSemana = concluidosSemana;

        return View(treinos);
    }

    public IActionResult Desempenho()
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

        var alunoId = _context.Alunos
            .Where(a => a.UsuarioId == usuarioId)
            .Select(a => a.Id)
            .First();

        var inicioSemana = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1);

        var dias = new[] { "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado", "Domingo" };

        var dados = new DesempenhoSemanalViewModel();

        foreach (var dia in dias)
        {
            var dataDia = inicioSemana.AddDays(Array.IndexOf(dias, dia));

            var total = _context.ExerciciosConcluidos
                .Include(c => c.Exercicio)
                .ThenInclude(e => e!.Treino)
                .Count(c =>
                    c.Data.Date == dataDia.Date &&
                    c.Exercicio!.Treino!.AlunoId == alunoId
                );

            dados.Dias.Add(dia);
            dados.Quantidades.Add(total);
        }

        return View(dados);
    }

}
