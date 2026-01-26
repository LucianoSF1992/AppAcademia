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
    public DbSet<ExercicioConcluido> ExerciciosConcluidos { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Instrutor -> Usuario (SEM cascade)
        modelBuilder.Entity<Instrutor>()
            .HasOne(i => i.Usuario)
            .WithMany()
            .HasForeignKey(i => i.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        // Aluno -> Usuario (SEM cascade)
        modelBuilder.Entity<Aluno>()
            .HasOne(a => a.Usuario)
            .WithMany()
            .HasForeignKey(a => a.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        // Aluno -> Instrutor (SEM cascade)
        modelBuilder.Entity<Aluno>()
            .HasOne(a => a.Instrutor)
            .WithMany()
            .HasForeignKey(a => a.InstrutorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
