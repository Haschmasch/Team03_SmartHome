using LanguageExt.Common;
using MainUnit.Models.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MainUnit.Extensions
{
    public static class ControllerExtension
    {
        public static IActionResult CheckForErrors<TResult>(this Result<TResult> result)
        {
            return result.Match<IActionResult>(obj => new OkObjectResult(obj), exception =>
            {
                if (exception is FluentValidation.ValidationException validationException)
                {
                    return new BadRequestObjectResult(validationException);
                }
                
                if (exception is UserAlreadyExistsException)
                {
                    return new BadRequestObjectResult(exception.Message);
                }

                return new StatusCodeResult(500);
            });
        }
    }
}
