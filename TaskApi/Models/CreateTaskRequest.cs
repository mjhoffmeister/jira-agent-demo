namespace TaskApi.Models;

public class CreateTaskRequest
{
    // BUG: No validation attributes — title can be null/empty,
    // description can be arbitrarily long, status unchecked.
    public string? Title { get; set; }
    public string? Description { get; set; }
}
