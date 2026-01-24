using Microsoft.AspNetCore.Mvc;
using AppAcademia.Filters;

namespace AppAcademia.Controllers;

[ServiceFilter(typeof(AuthFilter))]
public class InstrutorController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
