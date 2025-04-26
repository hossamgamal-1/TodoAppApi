using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoAppApi.Dtos;
using TodoAppApi.Helpers;
using TodoAppApi.Services;

namespace TodoAppApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : AppController
{
    private readonly IAuthService _authService = authService;

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequestDto dto) => await Handle(() => _authService.RegisterUserAsync(dto));

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync(LoginRequestDto dto) => await Handle(() => _authService.LoginAsync(dto));

    [Authorize]
    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto dto)
    {
        return await Handle(async () => {
            var jwt = HttpContext.Request.Headers.Authorization.ToString().Split(" ").Last();
            await _authService.ChangePasswordAsync(jwt,dto);
            return new { message = "Password changed successfully" };
        });
    }
}