using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoAppApi.Dtos;
using TodoAppApi.Helpers;
using TodoAppApi.Models;

namespace TodoAppApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController(ApplicationDbContext context,UserManager<AppUser> userManager) : AppController(userManager)
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<List<Tag>>> GetAll()
        {
            var user = await GetUserByTokenAsync();

            if(user is null)
                return Unauthorized();

            return Ok(await _context.Tags.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Tag>> PostTag(TagDto dto)
        {
            AppUser? user = await GetUserByTokenAsync();
            if(user is null)
                return Unauthorized();

            var tag = new Tag {
                Title = dto.Title,
            };

            _context.Tags.Add(tag);

            await _context.SaveChangesAsync();

            return Ok(tag);
        }
    }
}