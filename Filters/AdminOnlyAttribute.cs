using Microsoft.AspNetCore.Mvc.Filters;

namespace AppAcademia.Filters;

public class AdminOnlyAttribute : PerfilFilter
{
    public AdminOnlyAttribute() : base("Admin") { }
}
