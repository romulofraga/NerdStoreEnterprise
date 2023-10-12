using System.Text.RegularExpressions;

namespace NSE.Core.DomainObjects;

public class Email
{
    public const int EmailMaxLength = 245;
    public const int EmailMinLength = 5;

    protected Email()
    {
    }

    public Email(string endereco)
    {
        if (!Validar(endereco)) throw new DomainException("E-mail invalido");
        Endereco = endereco;
    }

    public string Endereco { get; set; }


    public static bool Validar(string email)
    {
        // Regular expression pattern for email validation
        var pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

        // Check if the email matches the pattern
        var match = Regex.Match(email, pattern);

        return match.Success;
    }
}