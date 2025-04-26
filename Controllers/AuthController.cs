using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoAppApi.Dtos;
using TodoAppApi.Helpers;
using TodoAppApi.Models;
using TodoAppApi.Services;

namespace TodoAppApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService,UserManager<AppUser> userManager) : AppController(userManager)
{
    private readonly IAuthService _authService = authService;

    [HttpPost("Register")]
    public async Task<ActionResult<AuthResponseDto>> RegisterAsync(RegisterRequestDto dto) => await Handle(() => _authService.RegisterUserAsync(dto));

    [HttpPost("Login")]
    public async Task<ActionResult<AuthResponseDto>> LoginAsync(LoginRequestDto dto) => await Handle(() => _authService.LoginAsync(dto));

    [Authorize]
    [HttpPost("ChangePassword")]
    public async Task<ActionResult<string>> ChangePasswordAsync(ChangePasswordDto dto)
    {
        return await Handle(async () => {
            await _authService.ChangePasswordAsync(GetToken(),dto);
            return "Password changed successfully";
        });
    }
}