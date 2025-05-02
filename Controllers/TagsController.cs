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
        public async Task<ActionResult<List<TodoTask>>> GetAll()
        {
            var user = await GetUserByTokenAsync();

            if(user is null)
                return Unauthorized();

            return Ok(await _context.Tags.ToListAsync());
        }

        //[HttpPost]
        //public async Task<ActionResult<TodoTask>> PostTag(TodoTaskDto dto)
        //{
        //    AppUser? user = await GetUserByTokenAsync();
        //    if(user is null)
        //        return Unauthorized();

        //    var todoTask = TodoTask.FromDto(dto,user);

        //    _context.TodoTasks.Add(todoTask);

        //    await _context.SaveChangesAsync();

        //    return Ok(todoTask);
        //}


        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTodoTask(int id)
        //{
        //    var user = await GetUserByTokenAsync();

        //    if(user is null)
        //        return Unauthorized();

        //    var todoTask = await _context.TodoTasks.FindAsync(id);
        //    if(todoTask == null || todoTask.User == null)
        //        return NotFound();

        //    if(todoTask.User.Id != user.Id)
        //        return Unauthorized();

        //    _context.TodoTasks.Remove(todoTask);
        //    await _context.SaveChangesAsync();

        //    return Ok(todoTask);
        //}
    }
}