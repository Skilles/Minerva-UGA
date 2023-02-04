namespace Minerva.Features.Authentication.Records;

public record struct LoginUserRequest(string Email, string Password);