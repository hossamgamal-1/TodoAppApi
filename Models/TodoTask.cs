using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TodoAppApi.Dtos;

namespace TodoAppApi.Models;

public class TodoTask : TodoTaskDto
{
    public int Id { get; set; }

    [NotMapped]
    public bool IsCompleted => CompletedAt is not null;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    [JsonIgnore]
    public AppUser? User { get; set; }

    public static TodoTask FromDto(TodoTaskDto dto,AppUser user)
    {
        return new TodoTask {
            Title = dto.Title,
            Description = dto.Description,
            User = user
        };
    }
}