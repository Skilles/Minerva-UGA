namespace Minerva.Validation;

public readonly record struct ValidationFailureResponse(List<string> Errors);