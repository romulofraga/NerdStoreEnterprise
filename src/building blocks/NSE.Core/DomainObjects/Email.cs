using System.Text.RegularExpressions;

namespace NSE.Core.DomainObjects
{
    public class Email
    {
        public const int EmailMaxLength = 245;
        public const int EmailMinLength = 5;

        public string Endereco { get; set; }

        protected Email()
        {

        }

        public Email(string endereco)
        {
            if (!Validar(endereco)) throw new DomainException("E-mail invalido");
            Endereco = endereco;
        }


        public static bool Validar(string email)
        {
            // Regular expression pattern for email validation
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            // Check if the email matches the pattern
            Match match = Regex.Match(email, pattern);

            return match.Success;
        }

    }
}
