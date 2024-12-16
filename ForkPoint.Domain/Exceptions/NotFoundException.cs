namespace ForkPoint.Domain.Exceptions;

public class NotFoundException(string resourceType, int resourceIdentifier)
    : Exception($"{resourceType} associated with id {resourceIdentifier} cannot be found")
{
}