using TodoAppApi.Models;

namespace TodoAppApi.Dtos;

public class AuthResponseDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Token { get; set; }

    public static AuthResponseDto FromUser(AppUser user,string token)
    {
        return new AuthResponseDto {
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName!,
            Email = user.Email!,
            Token = token
        };
    }
}