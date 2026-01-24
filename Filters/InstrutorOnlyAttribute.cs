using Microsoft.AspNetCore.Mvc.Filters;

namespace AppAcademia.Filters;

public class InstrutorOnlyAttribute : PerfilFilter
{
    public InstrutorOnlyAttribute() : base("Instrutor") { }
}
