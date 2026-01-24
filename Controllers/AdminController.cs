using Microsoft.AspNetCore.Mvc;
using AppAcademia.Filters;

namespace AppAcademia.Controllers;

[ServiceFilter(typeof(AuthFilter))]
public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
