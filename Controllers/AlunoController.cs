using AppAcademia.Data;
using AppAcademia.Models;
using AppAcademia.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAcademia.Controllers
{
    public class AlunoController : Controller
    {
        private readonly AppDbContext _context;

        public AlunoController(AppDbContext context)
        {
            _context = context;
        }

        // ===============================
        // DASHBOARD / TREINO DO DIA
        // ===============================
        public IActionResult Index()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
                return RedirectToAction("Login", "Auth");

            var alunoId = _context.Alunos
                .Where(a => a.UsuarioId == usuarioId)
                .Select(a => a.Id)
                .FirstOrDefault();

            var diaSemana = DateTime.Today.DayOfWeek.ToString();

            var treinos = _context.Treinos
                .Include(t => t.Exercicios)
                .Where(t =>
                    t.AlunoId == alunoId &&
                    t.Ativo &&
                    t.DiaSemana == diaSemana
                )
                .ToList();

            var concluidos = _context.ExerciciosConcluidos
                .Where(c =>
                    c.Exercicio != null &&
                    c.Exercicio.Treino != null &&
                    c.Exercicio.Treino.AlunoId == alunoId &&
                    c.DataConclusao.Date == DateTime.Today
                )
                .Select(c => c.ExercicioId)
                .ToList();

            ViewBag.Concluidos = concluidos;

            return View(treinos);
        }

        public IActionResult Dashboard()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
                return RedirectToAction("Login", "Auth");

            var alunoId = _context.Alunos
                .Where(a => a.UsuarioId == usuarioId)
                .Select(a => a.Id)
                .First();

            var inicioSemana = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var fimSemana = inicioSemana.AddDays(7);

            // Total planejado na semana
            var totalPlanejado = _context.Treinos
                .Where(t => t.AlunoId == alunoId && t.Ativo)
                .SelectMany(t => t.Exercicios)
                .Count();

            // Total concluído na semana
            var totalConcluido = _context.ExerciciosConcluidos
                .Where(c =>
                    c.Exercicio != null &&
                    c.Exercicio.Treino != null &&
                    c.Exercicio.Treino.AlunoId == alunoId &&
                    c.DataConclusao >= inicioSemana &&
                    c.DataConclusao < fimSemana
                )
                .Count();

            // Concluído hoje
            var feitoHoje = _context.ExerciciosConcluidos
                .Where(c =>
                    c.Exercicio != null &&
                    c.Exercicio.Treino != null &&
                    c.Exercicio.Treino.AlunoId == alunoId &&
                    c.DataConclusao.Date == DateTime.Today
                )
                .Count();

            var percentual = totalPlanejado == 0
                ? 0
                : (int)Math.Round((double)totalConcluido / totalPlanejado * 100);

            ViewBag.TotalPlanejado = totalPlanejado;
            ViewBag.TotalConcluido = totalConcluido;
            ViewBag.FeitoHoje = feitoHoje;
            ViewBag.Percentual = percentual;

            return View();
        }


        // ===============================
        // MARCAR EXERCÍCIO COMO CONCLUÍDO
        // ===============================
        [HttpPost]
        public IActionResult ConcluirExercicio(int exercicioId)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
                return RedirectToAction("Login", "Auth");

            var alunoId = _context.Alunos
                .Where(a => a.UsuarioId == usuarioId)
                .Select(a => a.Id)
                .First();

            var jaConcluido = _context.ExerciciosConcluidos.Any(c =>
                c.ExercicioId == exercicioId &&
                c.Exercicio != null &&
                c.Exercicio.Treino != null &&
                c.Exercicio.Treino.AlunoId == alunoId &&
                c.DataConclusao.Date == DateTime.Today
            );

            if (!jaConcluido)
            {
                _context.ExerciciosConcluidos.Add(new ExercicioConcluido
                {
                    ExercicioId = exercicioId,
                    DataConclusao = DateTime.Now
                });

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // ===============================
        // HISTÓRICO SEMANAL + PROGRESSO
        // ===============================
        public IActionResult Historico()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
                return RedirectToAction("Login", "Auth");

            var alunoId = _context.Alunos
                .Where(a => a.UsuarioId == usuarioId)
                .Select(a => a.Id)
                .First();

            var inicioSemana = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var fimSemana = inicioSemana.AddDays(7);

            // ===============================
            // HISTÓRICO DIÁRIO (AQUI ESTAVA FALTANDO)
            // ===============================
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

            // ===============================
            // TOTAL PLANEJADO
            // ===============================
            var totalPlanejado = _context.Treinos
                .Where(t =>
                    t.AlunoId == alunoId &&
                    t.Ativo
                )
                .SelectMany(t => t.Exercicios)
                .Count();

            // ===============================
            // TOTAL CONCLUÍDO NA SEMANA
            // ===============================
            var totalConcluido = _context.ExerciciosConcluidos
                .Where(c =>
                    c.Exercicio != null &&
                    c.Exercicio.Treino != null &&
                    c.Exercicio.Treino.AlunoId == alunoId &&
                    c.DataConclusao >= inicioSemana &&
                    c.DataConclusao < fimSemana
                )
                .Count();

            var percentual = totalPlanejado == 0
                ? 0
                : (int)Math.Round((double)totalConcluido / totalPlanejado * 100);

            ViewBag.InicioSemana = inicioSemana;
            ViewBag.FimSemana = fimSemana;
            ViewBag.TotalPlanejado = totalPlanejado;
            ViewBag.TotalConcluido = totalConcluido;
            ViewBag.Percentual = percentual;

            // ✅ AGORA EXISTE
            return View(historico);
        }
    }
}
