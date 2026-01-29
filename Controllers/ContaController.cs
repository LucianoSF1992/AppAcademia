using AppAcademia.Data;
using AppAcademia.Helpers;
using AppAcademia.ViewModels;
using Microsoft.AspNetCore.Mvc;
using TreineMais.ViewModels;


namespace AppAcademia.Controllers
{
    public class ContaController : Controller
    {
        private readonly AppDbContext _context;

        public ContaController(AppDbContext context)
        {
            _context = context;
        }

        // GET
        public IActionResult AlterarSenha()
        {
            return View();
        }

        // POST
        [HttpPost]
        public IActionResult AlterarSenha(AlterarSenhaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
                return RedirectToAction("Login", "Auth");

            var usuario = _context.Usuarios.Find(usuarioId);
            if (usuario == null)
                return RedirectToAction("Login", "Auth");

            // validar senha atual
            if (!PasswordHelper.Verificar(model.SenhaAtual, usuario.Senha))
            {
                ModelState.AddModelError("", "Senha atual incorreta");
                return View(model);
            }

            // atualizar senha
            usuario.Senha = PasswordHelper.Hash(model.NovaSenha);
            _context.SaveChanges();

            TempData["Sucesso"] = "Senha alterada com sucesso!";
            return RedirectToAction("AlterarSenha");
        }
    }
}
