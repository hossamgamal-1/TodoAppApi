﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TodoAppApi.Dtos;

public class TodoTaskDto
{
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(350)]
    public string Description { get; set; } = string.Empty;

    public ICollection<int> TagIds { get; set; } = [];
}