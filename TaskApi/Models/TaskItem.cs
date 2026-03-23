namespace TaskApi.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // BUG: Status is a raw string instead of an enum.
    // Callers can set it to anything — "doen", "in progrss", etc.
    public string Status { get; set; } = "todo";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}
