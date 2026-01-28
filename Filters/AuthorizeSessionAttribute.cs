using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppAcademia.Filters
{
    public class AuthorizeSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var usuarioId = context.HttpContext.Session.GetInt32("UsuarioId");

            if (!usuarioId.HasValue)
            {
                context.Result = new RedirectToActionResult(
                    "Login", "Auth", null
                );
            }
        }
    }
}
