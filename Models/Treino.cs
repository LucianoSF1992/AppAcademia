namespace AppAcademia.Models;

public class Treino
{
    public int Id { get; set; }
    public int AlunoId { get; set; }
    public Aluno? Aluno { get; set; }

    public string DiaSemana { get; set; } = string.Empty;
    public List<Exercicio> Exercicios { get; set; } = new();

    public bool Ativo { get; set; } = true;

}
