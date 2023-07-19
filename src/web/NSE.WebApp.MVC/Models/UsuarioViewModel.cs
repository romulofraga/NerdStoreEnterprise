using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NSE.WebApp.MVC.Extensions;

namespace NSE.WebApp.MVC.Models;

public class UsuarioRegistro
{
    [Required(ErrorMessage = "O campo {0} e obrigatorio")]
    [DisplayName("Nome Completo")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo {0} e obrigatorio")]
    [DisplayName("CPF")]
    [Cpf]
    public string Cpf { get; set; }

    [Required(ErrorMessage = "O campo {0} e obrigatorio")]
    [EmailAddress(ErrorMessage = "O campo {0} esta em formato invalido")]
    [DisplayName("E-mail")]
    public string Email { get; set; }

    [Required(ErrorMessage = "O campo {0} e obrigatorio")]
    [StringLength(100, ErrorMessage = "O campo precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
    public string Senha { get; set; }

    [Required(ErrorMessage = "O campo {0} e obrigatorio")]
    [Compare("Senha", ErrorMessage = "As senhas nao conferem.")]
    [DisplayName("Confirme sua senha")]
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

public class UsuarioResponse
{
    public string AccessToken { get; set; }
    public double ExpiresIn { get; set; }
    public UsuarioToken UsuarioToken { get; set; }
    public ResponseResult ResponseResult { get; set; }
}

public class UsuarioToken
{
    public string Id { get; set; }
    public string Email { get; set; }
    public IEnumerable<UsuarioClaim> Claims { get; set; }
}

public class UsuarioClaim
{
    public string Type { get; set; }
    public string Value { get; set; }
}