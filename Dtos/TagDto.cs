using System.ComponentModel.DataAnnotations;

namespace TodoAppApi.Dtos;

public class TagDto
{
    [MaxLength(256)]
    public required string Title { get; set; }
}