using System.ComponentModel.DataAnnotations;

namespace AppAcademia.ViewModels
{
    public class AlterarSenhaViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha atual")]
        public string SenhaAtual { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        [Display(Name = "Nova senha")]
        public string NovaSenha { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NovaSenha", ErrorMessage = "As senhas não conferem")]
        [Display(Name = "Confirmar nova senha")]
        public string ConfirmarSenha { get; set; }
    }
}
