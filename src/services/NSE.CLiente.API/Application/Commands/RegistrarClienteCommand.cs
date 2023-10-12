using FluentValidation;
using NSE.Core.Messages;

namespace NSE.Clientes.API.Application.Commands;

public class RegistrarClienteCommand : Command
{
    public RegistrarClienteCommand(Guid id, string nome, string email, string cpf)
    {
        AggregateId = id;
        Id = id;
        Nome = nome;
        Email = email;
        Cpf = cpf;
    }

    public Guid Id { get; }
    public string Nome { get; }
    public string Email { get; }
    public string Cpf { get; }

    public override bool IsValid()
    {
        ValidationResult = new RegistrarClienteValidation().Validate(this);

        return ValidationResult.IsValid;
    }

    public class RegistrarClienteValidation : AbstractValidator<RegistrarClienteCommand>
    {
        public RegistrarClienteValidation()
        {
            RuleFor(c => c.Nome)
                .NotEmpty()
                .WithMessage("O nome não foi informado.");

            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente inválido");

            RuleFor(c => c.Email)
                .Must(TerEmailValido)
                .WithMessage("O endereço de e-mail não é válido.");

            RuleFor(c => c.Cpf)
                .Must(TerCpfValido)
                .WithMessage("O CPF não é válido.");
        }

        protected static bool TerCpfValido(string cpf)
        {
            return Core.DomainObjects.Cpf.Validar(cpf);
        }

        protected static bool TerEmailValido(string email)
        {
            return Core.DomainObjects.Email.Validar(email);
        }
    }
}