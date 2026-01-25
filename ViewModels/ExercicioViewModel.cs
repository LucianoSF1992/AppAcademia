namespace AppAcademia.ViewModels;

public class ExercicioViewModel
{
    public string Nome { get; set; } = string.Empty;
    public string GrupoMuscular { get; set; } = string.Empty;

    public int Series { get; set; }
    public int Repeticoes { get; set; }
    public int Descanso { get; set; }

    public string? Observacoes { get; set; }
}
