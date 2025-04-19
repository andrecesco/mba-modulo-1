using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace MLV.ApiRest.Api;

[ApiController]
public abstract class MainController : ControllerBase
{
    protected ICollection<string> Erros = [];

    protected ActionResult CustomResponse(object result = null)
    {
        if (IsValid())
        {
            return Ok(result);
        }

        return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Mensagens", Erros.ToArray() }
            }));
    }

    //protected ActionResult CustomResponse(ModelStateDictionary modelState)
    //{
    //    var erros = modelState.Values.SelectMany(e => e.Errors);
    //    foreach (var erro in erros)
    //    {
    //        AddError(erro.ErrorMessage);
    //    }

    //    return CustomResponse();
    //}

    protected ActionResult CustomResponse(ValidationResult validationResult)
    {
        foreach (var erro in validationResult.Errors)
        {
            AddError(erro.ErrorMessage);
        }

        return CustomResponse();
    }

    //protected ActionResult CustomResponse(ResponseResultError resposta)
    //{
    //    ResponsePossuiErros(resposta);

    //    return CustomResponse();
    //}

    //protected bool ResponsePossuiErros(ResponseResultError resposta)
    //{
    //    if (resposta == null || resposta.Errors.Mensagens.Count == 0) return false;

    //    foreach (var mensagem in resposta.Errors.Mensagens)
    //    {
    //        AddError(mensagem);
    //    }

    //    return true;
    //}

    protected bool IsValid()
    {
        return Erros.Count == 0;
    }

    protected void AddError(string erro)
    {
        Erros.Add(erro);
    }

    //protected void ClearErros()
    //{
    //    Erros.Clear();
    //}
}
