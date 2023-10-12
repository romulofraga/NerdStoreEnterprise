using FluentValidation.Results;
using NSE.Core.Data;

namespace NSE.Core.Messages;

public abstract class CommandHandler
{
    protected ValidationResult ValidationResult;

    protected CommandHandler()
    {
        ValidationResult = new ValidationResult();
    }

    protected void AdicionarErro(string mensagemDeErro)
    {
        ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagemDeErro));
    }

    protected async Task<ValidationResult> PersistirDados(IUnityOfWork uow)
    {
        if (!await uow.Commit()) AdicionarErro("Um erro ocorreu ao persistir os dados.");

        return ValidationResult;
    }
}