using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;
using System.Net;

namespace MyRecipeBook.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;
    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }
    public void OnException(ExceptionContext context)
    {

        if (context.Exception is MyRecipeBookException myRecipeBookException)
            HandleProjectException(myRecipeBookException, context);
        else
            ThrowUnknowException(context);

        _logger.Log(LogLevel.Information, context.Exception.Message, "Erro capturado no Filter Log");
    }

    private static void HandleProjectException(MyRecipeBookException myRecipeBookException, ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)myRecipeBookException.GetStatusCode();
        context.Result = new ObjectResult(new ResponseError(myRecipeBookException.GetErrorMessages()));
        //Refatorado para não quebrar o pricipio de aberto e fechado do solid lá na ExceptionFilter
        //if(context.Exception is InvalidLoginException)
        //{
        //    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //    context.Result = new UnauthorizedObjectResult(new ResponseError(context.Exception.Message));
        //}
        //else if (context.Exception is ErrorOnValidationException)
        //{
        //    var exception = context.Exception as ErrorOnValidationException;

        //    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //    context.Result = new BadRequestObjectResult(new ResponseError(exception!.ErrorMessages));
        //}
        //else if(context.Exception is NotFoundException){
        //    context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        //    context.Result = new NotFoundObjectResult(new ResponseError(context.Exception.Message));
        //}
    }

    private static void ThrowUnknowException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new ResponseError(ResourceMessagesException.Unknow_Error));
    }
}
