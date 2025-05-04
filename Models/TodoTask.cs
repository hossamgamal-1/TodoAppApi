using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TodoAppApi.Dtos;

namespace TodoAppApi.Models;

public class TodoTask
{
    public int Id { get; set; }

    public bool IsCompleted => CompletedAt is not null;

    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(350)]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    [JsonIgnore]
    public AppUser? User { get; set; }

    public ICollection<Tag> Tags { get; set; } = [];
}