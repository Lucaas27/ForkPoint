namespace ForkPoint.Domain.Exceptions;
public class NotFoundException(string resourceType, int resourceIdentifier)
    : Exception($"{resourceType} with id {resourceIdentifier} does not exist.")
{
}
