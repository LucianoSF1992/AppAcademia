using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppAcademia.Filters;

public class PerfilFilter : IActionFilter
{
    private readonly string _perfilPermitido;

    public PerfilFilter(string perfilPermitido)
    {
        _perfilPermitido = perfilPermitido;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var perfil = context.HttpContext.Session.GetString("Perfil");

        if (perfil == null || perfil != _perfilPermitido)
        {
            context.Result = new RedirectToActionResult("Login", "Auth", null);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // não usado
    }
}
