using Microsoft.AspNetCore.Mvc;
using AppAcademia.Filters;

namespace AppAcademia.Controllers;

[ServiceFilter(typeof(AdminOnlyAttribute))]
public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
