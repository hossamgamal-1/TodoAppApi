using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TodoAppApi.Models;

public class Tag
{
    public int Id { get; set; }

    [MaxLength(256)]
    public required string Title { get; set; }

    [JsonIgnore]

    public ICollection<TodoTask> TodoTasks { get; set; } = [];
}