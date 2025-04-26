using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TodoAppApi.Dtos;

namespace TodoAppApi.Models;

public class AppUser : IdentityUser
{
    [MaxLength(50)]
    public required string FirstName { get; set; }

    [MaxLength(50)]
    public required string LastName { get; set; }

    public static AppUser FromRegisterDto(RegisterRequestDto dto)
    {
        return new AppUser {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            UserName = dto.UserName,
            Email = dto.Email,
        };
    }
}