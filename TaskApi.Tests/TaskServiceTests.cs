using System.ComponentModel.DataAnnotations;
using TaskApi.Models;
using TaskApi.Services;

namespace TaskApi.Tests;

public class TaskServiceTests
{
    // Helper to run data-annotation validation on a model instance.
    private static IList<ValidationResult> Validate(object model)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);
        return results;
    }

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

    [Fact]
    public void GetAll_TotalPages_UsesCeilingDivision()
    {
        var service = new TaskService();
        service.Create(new CreateTaskRequest { Title = "T1" });
        service.Create(new CreateTaskRequest { Title = "T2" });
        service.Create(new CreateTaskRequest { Title = "T3" });

        var result = service.GetAll(page: 1, pageSize: 2);

        Assert.Equal(2, result.TotalPages);
    }

    [Fact]
    public void GetAll_PageLessThanOne_ReturnsEmptyResult()
    {
        var service = new TaskService();
        service.Create(new CreateTaskRequest { Title = "T1" });

        var result = service.GetAll(page: 0, pageSize: 10);

        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
        Assert.Equal(0, result.TotalPages);
    }

    [Fact]
    public void GetAll_ReturnsCorrectItemsForPage()
    {
        var service = new TaskService();
        service.Create(new CreateTaskRequest { Title = "T1" });
        service.Create(new CreateTaskRequest { Title = "T2" });
        service.Create(new CreateTaskRequest { Title = "T3" });

        var page1 = service.GetAll(page: 1, pageSize: 2);
        var page2 = service.GetAll(page: 2, pageSize: 2);

        Assert.Equal(2, page1.Items.Count);
        Assert.Equal("T1", page1.Items[0].Title);
        Assert.Equal("T2", page1.Items[1].Title);

        Assert.Single(page2.Items);
        Assert.Equal("T3", page2.Items[0].Title);
    }

    [Fact]
    public void CreateTaskRequest_EmptyTitle_FailsValidation()
    {
        var request = new CreateTaskRequest { Title = "" };
        var results = Validate(request);
        Assert.NotEmpty(results);
    }

    [Fact]
    public void CreateTaskRequest_WhitespaceTitle_FailsValidation()
    {
        var request = new CreateTaskRequest { Title = "   " };
        var results = Validate(request);
        Assert.NotEmpty(results);
    }

    [Fact]
    public void CreateTaskRequest_ValidTitle_PassesValidation()
    {
        var request = new CreateTaskRequest { Title = "My Task" };
        var results = Validate(request);
        Assert.Empty(results);
    }
}
