using System.Collections.Generic;

namespace AppAcademia.ViewModels;

public class TreinoViewModel
{
    public int AlunoId { get; set; }
    public string DiaSemana { get; set; } = string.Empty;

    public List<ExercicioViewModel> Exercicios { get; set; } = new();
}
