namespace AppAcademia.Models;

public class ExercicioConcluido
{
    public int Id { get; set; }

    public int ExercicioId { get; set; }
    public Exercicio? Exercicio { get; set; }

    public DateTime Data { get; set; }
}
