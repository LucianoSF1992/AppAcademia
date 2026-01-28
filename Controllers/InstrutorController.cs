using AppAcademia.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAcademia.Controllers
{
    public class InstrutorController : Controller
    {
        private readonly AppDbContext _context;

        public InstrutorController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            var perfil = HttpContext.Session.GetString("Perfil");

            if (usuarioId == null || perfil != "Instrutor")
                return RedirectToAction("Login", "Auth");

            var instrutorId = _context.Instrutores
                .Where(i => i.UsuarioId == usuarioId)
                .Select(i => i.Id)
                .FirstOrDefault();

            // Alunos do instrutor
            var totalAlunos = _context.Alunos
                .Count(a => a.InstrutorId == instrutorId);

            // Treinos ativos
            var totalTreinos = _context.Treinos
                .Include(t => t.Aluno)
                .Where(t =>
                    t.Aluno != null &&
                    t.Aluno.InstrutorId == instrutorId &&
                    t.Ativo
    )
    .Count();



            // Exercícios cadastrados
            var totalExercicios = _context.Exercicios
                .Include(e => e.Treino)
                .ThenInclude(t => t.Aluno)
                .Where(e =>
                    e.Treino != null &&
                    e.Treino.Aluno != null &&
                    e.Treino.Aluno.InstrutorId == instrutorId
                                                                                                                                                                                                                                                    )
    .Count();




            // Treinos iniciados na semana
            var inicioSemana = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var fimSemana = inicioSemana.AddDays(7);

            var treinosIniciadosSemana = _context.ExerciciosConcluidos
                .Include(c => c.Exercicio)
                .ThenInclude(e => e.Treino)
                .ThenInclude(t => t.Aluno)
    .Where(c =>
        c.Exercicio != null &&
        c.Exercicio.Treino != null &&
        c.Exercicio.Treino.Aluno != null &&
        c.Exercicio.Treino.Aluno.InstrutorId == instrutorId &&
        c.DataConclusao >= inicioSemana &&
        c.DataConclusao < fimSemana
    )
    .Count();

    );


            ViewBag.TotalAlunos = totalAlunos;
            ViewBag.TotalTreinos = totalTreinos;
            ViewBag.TotalExercicios = totalExercicios;
            ViewBag.TreinosIniciadosSemana = treinosIniciadosSemana;

            return View();
        }
    }
}
