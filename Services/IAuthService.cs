using TodoAppApi.Dtos;

namespace TodoAppApi.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterUserAsync(RegisterRequestDto dto);

    Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);

    Task ChangePasswordAsync(string token,ChangePasswordDto dto);
}