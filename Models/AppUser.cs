using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TodoAppApi.Models;

public class AppUser : IdentityUser
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }
}