using TaskApi.Models;
using TaskStatus = TaskApi.Models.TaskStatus;

namespace TaskApi.Services;

public class TaskService
{
    // In-memory store for demo purposes.
    private readonly List<TaskItem> _tasks = [];
    private int _nextId = 1;

    public TaskItem Create(CreateTaskRequest request)
    {
        var task = new TaskItem
        {
            Id = _nextId++,
            Title = request.Title,
            Description = request.Description ?? string.Empty,
            Status = TaskStatus.Todo,
            CreatedAt = DateTime.UtcNow
        };

        _tasks.Add(task);
        return task;
    }

    public TaskItem? GetById(int id) => _tasks.FirstOrDefault(t => t.Id == id);

    public PagedResult<TaskItem> GetAll(int page, int pageSize)
    {
        if (page < 1)
            return new PagedResult<TaskItem>
            {
                Items = [],
                TotalCount = 0,
                Page = page,
                PageSize = pageSize,
                TotalPages = 0
            };

        var totalCount = _tasks.Count;
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var items = _tasks
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResult<TaskItem>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages
        };
    }

    public TaskItem? UpdateStatus(int id, TaskStatus newStatus)
    {
        var task = GetById(id);
        if (task is null) return null;

        task.Status = newStatus;

        if (newStatus == TaskStatus.Done)
            task.CompletedAt = DateTime.UtcNow;
        else
            task.CompletedAt = null;

        return task;
    }

    public bool Delete(int id)
    {
        var task = GetById(id);
        if (task is null) return false;
        _tasks.Remove(task);
        return true;
    }
}
