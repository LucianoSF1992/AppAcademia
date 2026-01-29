using System.ComponentModel.DataAnnotations;

namespace TreineMais.ViewModels
{
    public class AlterarSenhaViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string SenhaAtual { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "A nova senha deve ter no mínimo 6 caracteres")]
        public string NovaSenha { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("NovaSenha", ErrorMessage = "As senhas não conferem")]
        public string ConfirmarSenha { get; set; } = string.Empty;
    }
}
