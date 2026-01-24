using Microsoft.EntityFrameworkCore;
using AppAcademia.Models;

namespace AppAcademia.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Instrutor> Instrutores { get; set; }
    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<Treino> Treinos { get; set; }
    public DbSet<Exercicio> Exercicios { get; set; }
}
