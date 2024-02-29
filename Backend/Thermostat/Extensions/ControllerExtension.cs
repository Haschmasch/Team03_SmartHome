using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace Thermostat.Extensions
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

                return new StatusCodeResult(500);
            });
        }
    }
}
