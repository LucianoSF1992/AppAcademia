namespace AppAcademia.Models;

public class Exercicio
{
    public int Id { get; set; }
    public int TreinoId { get; set; }
    public Treino? Treino { get; set; }

    public string Nome { get; set; } = string.Empty;
    public string GrupoMuscular { get; set; } = string.Empty;
    public int Series { get; set; }
    public int Repeticoes { get; set; }
    public int Descanso { get; set; }
    public string Observacoes { get; set; } = string.Empty;
}
