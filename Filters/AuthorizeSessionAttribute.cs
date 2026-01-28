using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppAcademia.Filters
{
    public class AuthorizeSessionAttribute : ActionFilterAttribute
    {
        private readonly string? _perfil; // 👈 aqui está a correção

        public AuthorizeSessionAttribute(string? perfil = null)
        {
            _perfil = perfil;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;

            var usuarioId = httpContext.Session.GetInt32("UsuarioId");
            var perfil = httpContext.Session.GetString("Perfil");

            if (usuarioId == null)
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            if (_perfil != null && perfil != _perfil)
            {
                context.Result = new RedirectToActionResult("AcessoNegado", "Auth", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
