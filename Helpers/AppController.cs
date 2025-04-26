using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using TodoAppApi.Models;

namespace TodoAppApi.Helpers;

public class AppController(UserManager<AppUser> userManager) : ControllerBase
{
    private readonly UserManager<AppUser> _userManager = userManager;

    public async Task<ActionResult<T>> Handle<T>(Func<Task<T>> action)
    {
        try
        {
            var response = await action();
            return Ok(response);
        }
        catch(Exception ex)
        {
            return BadRequest(new { errorMessage = ex.Message });
        }
    }

    protected string GetToken()
    {
        Request.Headers.TryGetValue("Authorization",out var token);
        return token.ToString().Replace("Bearer ","");
    }

    protected async Task<AppUser?> GetUserByTokenAsync()
    {
        var jwt = GetToken();
        var payload = new JwtSecurityTokenHandler().ReadJwtToken(jwt).Payload;

        string? userEmail = payload.GetValueOrDefault("email") as string;

        if(string.IsNullOrEmpty(userEmail))
            return null;

        AppUser? user = await _userManager.FindByEmailAsync(userEmail);

        return user;
    }
}