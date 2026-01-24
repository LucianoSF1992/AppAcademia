using Microsoft.AspNetCore.Mvc.Filters;

namespace AppAcademia.Filters;

public class AlunoOnlyAttribute : PerfilFilter
{
    public AlunoOnlyAttribute() : base("Aluno") { }
}
