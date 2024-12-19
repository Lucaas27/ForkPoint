namespace ForkPoint.Domain.Exceptions;

public class NotFoundException(string resourceType, string resourceIdentifier)
    : Exception($"{resourceType} associated with identifier '{resourceIdentifier}' cannot be found")
{
}