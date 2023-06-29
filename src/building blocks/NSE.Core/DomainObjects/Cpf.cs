namespace NSE.Core.DomainObjects
{
    public class Cpf
    {
        public const int CpfMaxLength = 11;
        public string Numero { get; private set; }

        protected Cpf() { }

        public Cpf(string numero)
        {
            if (!Validar(numero)) throw new DomainException("CPF Invalido");
            Numero = numero;
        }

        public static bool Validar(string cpf)
        {
            // Remove any non-digit characters from the input
            string digits = new(cpf.Where(char.IsDigit).ToArray());

            // Check if the CPF has the correct length
            if (digits.Length != 11)
            {
                return false;
            }

            // Check if all digits are the same (e.g., 00000000000)
            bool allSameDigits = digits.Distinct().Count() == 1;
            if (allSameDigits)
            {
                return false;
            }

            // Calculate the verification digits
            int[] factors = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] cpfDigits = digits.Take(9).Select(c => int.Parse(c.ToString())).ToArray();
            int[] firstVerifier = CalculateVerifiers(cpfDigits, factors);
            int[] secondVerifier = CalculateVerifiers(cpfDigits.Concat(firstVerifier).ToArray(), factors);

            // Compare the calculated verifiers with the provided ones
            return digits.Substring(9, 2) == $"{firstVerifier[0]}{secondVerifier[0]}";
        }

        private static int[] CalculateVerifiers(int[] digits, int[] factors)
        {
            int sum = 0;

            for (int i = 0; i < digits.Length; i++)
            {
                sum += digits[i] * factors[i];
            }

            int remainder = sum % 11;
            int verifier = remainder < 2 ? 0 : 11 - remainder;

            return new[] { verifier };
        }

    }
}
