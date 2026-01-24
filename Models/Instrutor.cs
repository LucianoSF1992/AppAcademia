namespace AppAcademia.Models;

public class Instrutor
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }
}
