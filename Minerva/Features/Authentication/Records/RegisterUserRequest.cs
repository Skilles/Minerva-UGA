namespace Minerva.Features.Authentication.Records;

public record struct RegisterUserRequest(string FirstName, string LastName, string Password, string Email);