using Microsoft.AspNetCore.Mvc;
using AppAcademia.Filters;

namespace AppAcademia.Controllers;

[ServiceFilter(typeof(AuthFilter))]
public class AlunoController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
