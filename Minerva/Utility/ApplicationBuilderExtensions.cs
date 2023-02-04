using Minerva.Validation;

namespace Minerva.Utility;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseValidationException(this IApplicationBuilder app) => app.UseMiddleware<ValidationExceptionMiddleware>();
}