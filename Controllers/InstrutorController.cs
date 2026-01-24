using Microsoft.AspNetCore.Mvc;
using AppAcademia.Filters;

namespace AppAcademia.Controllers;

[ServiceFilter(typeof(InstrutorOnlyAttribute))]
public class InstrutorController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
