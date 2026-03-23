using TaskApi.Models;
using TaskApi.Services;

namespace TaskApi.Tests;

public class TaskServiceTests
{
    [Fact]
    public void Create_ReturnsTaskWithId()
    {
        var service = new TaskService();
        var task = service.Create(new CreateTaskRequest { Title = "Test", Description = "Desc" });

        Assert.NotEqual(0, task.Id);
        Assert.Equal("Test", task.Title);
        Assert.Equal("todo", task.Status);
    }

    [Fact]
    public void GetById_ReturnsNull_WhenNotFound()
    {
        var service = new TaskService();
        Assert.Null(service.GetById(999));
    }

    // TODO: No tests for pagination, validation, or status updates.
    // This is intentionally sparse — a Jira issue will ask the coding
    // agent to add comprehensive test coverage.
}
