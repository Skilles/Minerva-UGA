namespace Minerva.Validation;

public class MinervaValidationException : Exception
{
    public MinervaValidationException(string message) : base(message) { }
}