namespace Cortexa.Application.Dtos.Admin
{
    public record UserRoleDto(string UserId, string UserName, List<string> Roles);
}