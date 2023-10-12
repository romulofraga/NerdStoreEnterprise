using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSE.Core.Comunication;

namespace NSE.WebApi.Core.Controllers;

[ApiController]
public abstract class MainController : ControllerBase
{
    protected ICollection<string> Erros = new List<string>();

    protected ActionResult CustomResponse(object result = null)
    {
        if (OperacaoValida()) return Ok(result);

        return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            { "Mensagens", Erros.ToArray() }
        }));
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelstate)
    {
        var errors = modelstate.Values.SelectMany(error => error.Errors);
        foreach (var error in errors) AdicionarErroProcessamento(error.ErrorMessage);

        return CustomResponse();
    }

    protected ActionResult CustomResponse(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors) AdicionarErroProcessamento(error.ErrorMessage);

        return CustomResponse();
    }

    protected ActionResult CustomResponse(ResponseResult resposta)
    {
        ResponsePossuiErros(resposta);

        return CustomResponse();
    }

    protected bool ResponsePossuiErros(ResponseResult resposta)
    {
        if (resposta == null || !resposta.Errors.Mensagens.Any()) return false;

        foreach (var mensagem in resposta.Errors.Mensagens) AdicionarErroProcessamento(mensagem);

        return true;
    }

    protected bool OperacaoValida()
    {
        return !Erros.Any();
    }

    protected bool OperacaoInvalida()
    {
        return !OperacaoValida();
    }

    protected void AdicionarErroProcessamento(string erro)
    {
        Erros.Add(erro);
    }

    protected void LimparerrosProcessamento()
    {
        Erros.Clear();
    }
}