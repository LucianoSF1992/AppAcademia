namespace AppAcademia.Models;

public class Treino
{
    public int Id { get; set; }
    public int AlunoId { get; set; }
    public Aluno? Aluno { get; set; }
    public DayOfWeek DiaSemana { get; set; }

    public List<Exercicio> Exercicios { get; set; } = new();

    public bool Ativo { get; set; } = true;

}
