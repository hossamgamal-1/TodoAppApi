using System.ComponentModel.DataAnnotations;

namespace TodoAppApi.Models;

public class Tag
{
    public int Id { get; set; }

    [MaxLength(256)]
    public required string title { get; set; }

    public ICollection<TodoTask> TodoTasks { get; set; } = [];
}