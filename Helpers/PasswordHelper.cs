using BCrypt.Net;

namespace AppAcademia.Helpers
{
    public static class PasswordHelper
    {
        public static string Hash(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public static bool Verificar(string senhaDigitada, string hashBanco)
        {
            return BCrypt.Net.BCrypt.Verify(senhaDigitada, hashBanco);
        }
    }
}
