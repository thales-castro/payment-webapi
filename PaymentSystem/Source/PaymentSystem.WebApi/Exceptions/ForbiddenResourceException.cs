namespace PaymentSystem.WebApi.Exceptions;

public class ForbiddenResourceException : Exception
{
    public ForbiddenResourceException()
    {
    }

    public ForbiddenResourceException(string? message) : base(message)
    {
    }

    public ForbiddenResourceException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
