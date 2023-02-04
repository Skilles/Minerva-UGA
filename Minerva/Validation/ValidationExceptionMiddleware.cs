using FluentValidation;

namespace Minerva.Validation;

public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate Request;

    public ValidationExceptionMiddleware(RequestDelegate request)
    {
        Request = request;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await Request(context);
        }
        catch (ValidationException e)
        {
            context.Response.StatusCode = 400;
            var messages = e.Errors.Select(x => x.ErrorMessage).ToList();
            var validationFailureResponse = new ValidationFailureResponse
            {
                Errors = messages
            };
            await context.Response.WriteAsJsonAsync(validationFailureResponse);
        }
        catch (MinervaValidationException e)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { error = e.Message });
            await Console.Error.WriteLineAsync("MinervaValidationException: " + e.Message);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { error = e.Message });
            throw;
        }
    }
}