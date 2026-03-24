using System.ComponentModel.DataAnnotations;

namespace TaskApi.Models;

public class CreateTaskRequest
{
    [Required]
    [MinLength(1)]
    public required string Title { get; set; }
    public string? Description { get; set; }
}
