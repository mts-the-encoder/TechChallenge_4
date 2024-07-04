using System.Net;
using Communication.Responses;
using Exceptions;
using Exceptions.ExceptionBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

public class ExceptionsFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is TechChallengeException)
            TreatException(context);
        else
            ThrowUnknownError(context);
    }

    private static void TreatException(ExceptionContext context)
    {
        if (context.Exception is ValidationErrorsException)
            TreatValidationsException(context);

        else if (context.Exception is InvalidLoginException) TreatLoginException(context);
    }

    private static void TreatValidationsException(ExceptionContext context)
    {
        var errors = context.Exception as ValidationErrorsException;

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Result = new JsonResult(new ErrorResponse(errors.ErrorMessages));
    }

    private static void ThrowUnknownError(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new ErrorResponse("Erro desconhecido"));
    }

    private static void TreatLoginException(ExceptionContext context)
    {
        var error = context.Exception as InvalidLoginException;

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Result = new ObjectResult(new ErrorResponse(error.Message));
    }
}