using System.ComponentModel.DataAnnotations;

namespace NSE.WebApp.MVC.Models
{
    public class UsuarioRegistro
    {
        [Required(ErrorMessage = "O campo {0} e obrigatorio")]
        [EmailAddress(ErrorMessage = "O campo {0} esta em formato invalido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} e obrigatorio")]
        [StringLength(100, ErrorMessage = "O campo precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Senha { get; set; }

        [Required(ErrorMessage = "O campo {0} e obrigatorio")]
        [Compare("Senha", ErrorMessage = "As senhas nao conferem.")]
        public string SenhaConfirmacao { get; set; }
    }

    public class UsuarioLogin
    {
        [Required(ErrorMessage = "O campo {0} e obrigatorio")]
        [EmailAddress(ErrorMessage = "O campo {0} esta em formato invalido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} e obrigatorio")]
        [StringLength(100, ErrorMessage = "O campo precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Senha { get; set; }
    }
}
