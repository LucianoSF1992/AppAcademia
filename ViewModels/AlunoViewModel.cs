namespace AppAcademia.ViewModels;

public class AlunoViewModel
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;

    public int? Idade { get; set; }
    public string Objetivo { get; set; } = string.Empty;
}
