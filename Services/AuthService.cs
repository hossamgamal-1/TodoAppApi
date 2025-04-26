using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoAppApi.Dtos;
using TodoAppApi.Models;

namespace TodoAppApi.Services;

public class AuthService(UserManager<AppUser> userManager,IOptions<JWT> jwt) : IAuthService
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly JWT _jwt = jwt.Value;

    public async Task<AuthResponseDto> RegisterUserAsync(RegisterRequestDto dto)
    {
        if(await _userManager.FindByEmailAsync(dto.Email) is not null)
            throw new Exception("Email already exists");

        if(await _userManager.FindByNameAsync(dto.UserName) is not null)
            throw new Exception("UserName already exists");

        var user = AppUser.FromRegisterDto(dto);

        var result = await _userManager.CreateAsync(user,dto.Password);

        if(!result.Succeeded)
            throw new Exception(string.Join(", ",result.Errors.Select(e => e.Description)));

        string token = await CreateJwtToken(user);

        return AuthResponseDto.FromUser(user,token);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
    {
        AppUser user = await _userManager.FindByEmailAsync(dto.Email) ?? throw new Exception("Invalid Email or Password");

        bool isValid = await _userManager.CheckPasswordAsync(user,dto.Password);

        if(!isValid)
            throw new Exception("Invalid Email or Password");

        string token = await CreateJwtToken(user);

        return AuthResponseDto.FromUser(user,token);
    }

    public async Task ChangePasswordAsync(string token,ChangePasswordDto dto)
    {
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

        string userEmail = jwtToken.Payload.GetValueOrDefault("email") as string ?? throw new Exception("Invalid token");

        AppUser user = await _userManager.FindByEmailAsync(userEmail) ?? throw new Exception("Invalid token");

        var result = await _userManager.ChangePasswordAsync(user,dto.CurrentPassword,dto.NewPassword);

        if(!result.Succeeded)
            throw new Exception(string.Join(", ",result.Errors.Select(e => e.Description)));
    }

    private async Task<string> CreateJwtToken(AppUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();

        foreach(var role in roles)
            roleClaims.Add(new Claim("roles",role));

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, value: user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("uid", user.Id)
            }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(_jwt.DurationInDays),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        ;
    }
}