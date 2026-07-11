namespace Cortexa.Application.Dtos.Admin
{
    public record UserRoleDto(string UserId, string UserName,string Email, List<string> Roles);
}