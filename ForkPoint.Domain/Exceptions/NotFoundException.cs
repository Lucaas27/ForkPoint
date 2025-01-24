namespace ForkPoint.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string resourceType, string resourceIdentifier)
        : base($"{resourceType} associated with identifier '{resourceIdentifier}' cannot be found")
    {
    }

    public NotFoundException()
    {
    }

    public NotFoundException(string? message) : base(message)
    {
    }

    public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
