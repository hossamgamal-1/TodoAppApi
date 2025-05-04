using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAppApi.Dtos;
using TodoAppApi.Helpers;
using TodoAppApi.Models;

namespace TodoAppApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoTasksController(ApplicationDbContext context,UserManager<AppUser> userManager) : AppController(userManager)
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<List<TodoTask>>> GetAll()
        {
            var user = await GetUserByTokenAsync();

            if(user is null)
                return Unauthorized();
            var tasks = await _context.TodoTasks
                .Where(t => t.User != null && t.User.Id == user.Id)
                .OrderByDescending(t => t.CreatedAt)
                .Include(t => t.Tags)
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoTask>> GetTodoTask(int id)
        {
            var user = await GetUserByTokenAsync();

            if(user is null)
                return Unauthorized();

            TodoTask? todoTask = await _context.TodoTasks.FindAsync(id);

            if(todoTask == null || todoTask.User == null)
                return NotFound();

            return todoTask.User.Id != user.Id ? NotFound() : Ok(todoTask);
        }

        [HttpPost]
        public async Task<ActionResult<TodoTask>> PostTodoTask(TodoTaskDto dto)
        {
            AppUser? user = await GetUserByTokenAsync();
            if(user is null)
                return Unauthorized();

            var tags = new List<Tag>();

            foreach(int tagId in dto.TagIds)
            {
                Tag? tag = await _context.Tags.FindAsync(tagId);
                if(tag is null)
                    return NotFound($"Tag with id {tagId} not found");

                tags.Add(tag);
            }

            var todoTask = new TodoTask {
                Title = dto.Title,
                Description = dto.Description,
                User = user,
                Tags = tags
            };

            _context.TodoTasks.Add(todoTask);

            await _context.SaveChangesAsync();

            return Ok(todoTask);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TodoTask>> UpdateTodoTask(int id,[FromBody] TodoTaskDto dto)
        {
            AppUser? user = await GetUserByTokenAsync();
            if(user is null)
                return Unauthorized();

            var todoTask = await _context.TodoTasks.FindAsync(id);

            if(todoTask == null || todoTask.User == null)
                return NotFound();

            if(todoTask.User.Id != user.Id)
                return Unauthorized();

            todoTask.Title = string.IsNullOrEmpty(dto.Title) ? todoTask.Title : dto.Title;
            todoTask.Description = string.IsNullOrEmpty(dto.Description) ? todoTask.Description : dto.Description;
            await _context.SaveChangesAsync();

            return Ok(todoTask);
        }

        [HttpPatch("{id}/Completed")]
        public async Task<ActionResult<TodoTask>> SetIsCompleted(int id)
        {
            AppUser? user = await GetUserByTokenAsync();
            if(user is null)
                return Unauthorized();

            var todoTask = await _context.TodoTasks.FindAsync(id);

            if(todoTask == null || todoTask.User == null)
                return NotFound();

            if(todoTask.User.Id != user.Id)
                return Unauthorized();

            if(todoTask.CompletedAt != null)
                return Ok(todoTask);

            todoTask.CompletedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(todoTask);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoTask(int id)
        {
            var user = await GetUserByTokenAsync();

            if(user is null)
                return Unauthorized();

            var todoTask = await _context.TodoTasks.FindAsync(id);
            if(todoTask == null || todoTask.User == null)
                return NotFound();

            if(todoTask.User.Id != user.Id)
                return Unauthorized();

            _context.TodoTasks.Remove(todoTask);
            await _context.SaveChangesAsync();

            return Ok(todoTask);
        }
    }
}