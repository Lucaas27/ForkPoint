namespace ForkPoint.Application.Models.Dtos;

public record CurrentUserModel(int Id, string Email, IEnumerable<string> Roles)
{
    public bool IsInRole(string role)
    {
        return Roles.Contains(role);
    }
}