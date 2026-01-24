namespace AppAcademia.Models;

public class Aluno
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public int? Idade { get; set; }
    public string Objetivo { get; set; } = string.Empty;

    public int InstrutorId { get; set; }
    public Instrutor? Instrutor { get; set; }
}
