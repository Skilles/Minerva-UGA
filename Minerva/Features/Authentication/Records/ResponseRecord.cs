namespace Minerva.Features.Authentication.Records;

public record struct ResponseRecord<T>(T Data, string Message, int Code);