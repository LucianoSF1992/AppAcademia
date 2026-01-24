using AppAcademia.Models;

namespace AppAcademia.Data;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        try
        {
            if (context.Usuarios.Any())
                return;

            var admin = new Usuario
            {
                Nome = "Administrador",
                Email = "administratorappacademia@academia.com",
                Senha = "admin",
                Perfil = "Admin"
            };

            context.Usuarios.Add(admin);
            context.SaveChanges();
        }
        catch
        {
            // evita quebrar a aplicação se o banco estiver indisponível
        }
    }
}
