using AppAcademia.Data;
using AppAcademia.Models;
using AppAcademia.ViewModels;
using AppAcademia.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAcademia.Controllers
{
    [AuthorizeSession("Instrutor")]
    public class TreinoInstrutorController : Controller
    {
        private readonly AppDbContext _context;

        public TreinoInstrutorController(AppDbContext context)
        {
            _context = context;
        }

        // ===============================
        // LISTAR ALUNOS
        // ===============================
        public IActionResult Index()
        {
            var instrutorId = HttpContext.Session.GetInt32("UsuarioId");

            if (instrutorId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var alunos = _context.Alunos
                .Include(a => a.Usuario)
                .Include(a => a.Instrutor)
                .Where(a => a.Instrutor != null &&
                            a.Instrutor.UsuarioId == instrutorId.Value)
                .ToList();


            return View(alunos);
        }

        // ===============================
        // LISTAR TREINOS DO ALUNO
        // ===============================
        public IActionResult Detalhes(int alunoId)
        {
            var treinos = _context.Treinos
                .Include(t => t.Exercicios)
                .Where(t => t.AlunoId == alunoId)
                .ToList();

            // Ordenar: Segunda → Domingo
            var treinosOrdenados = treinos
                .OrderBy(t => ((int)t.DiaSemana + 6) % 7)
                .Select(t => new TreinoStatusViewModel
                {
                    TreinoId = t.Id,
                    DiaSemana = t.DiaSemana,
                    TotalExercicios = t.Exercicios.Count,
                    ExerciciosConcluidos = t.Exercicios.Count(e =>
                        _context.ExerciciosConcluidos.Any(c => c.ExercicioId == e.Id)
                    )
                })
                .ToList();

            ViewBag.AlunoId = alunoId;

            return View(treinosOrdenados);
        }

        // ===============================
        // FORMULÁRIO CRIAR TREINO
        // ===============================
        public IActionResult Create(int alunoId)
        {
            ViewBag.AlunoId = alunoId;
            return View();
        }

        // ===============================
        // SALVAR TREINO
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Treino treino)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AlunoId = treino.AlunoId;
                return View(treino);
            }

            _context.Treinos.Add(treino);
            _context.SaveChanges();

            return RedirectToAction("Detalhes", new { alunoId = treino.AlunoId });
        }

        // ===============================
        // EXCLUIR TREINO
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var treino = _context.Treinos
                .Include(t => t.Exercicios)
                .FirstOrDefault(t => t.Id == id);

            if (treino == null)
                return NotFound();

            // Remove exercícios primeiro
            _context.Exercicios.RemoveRange(treino.Exercicios);
            _context.Treinos.Remove(treino);
            _context.SaveChanges();

            return RedirectToAction("Detalhes", new { alunoId = treino.AlunoId });
        }
    }
}
