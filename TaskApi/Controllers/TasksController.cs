using Microsoft.AspNetCore.Mvc;
using TaskApi.Models;
using TaskApi.Services;

namespace TaskApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController(TaskService taskService) : ControllerBase
{
    [HttpGet]
    public ActionResult<PagedResult<TaskItem>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = taskService.GetAll(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public ActionResult<TaskItem> GetById(int id)
    {
        var task = taskService.GetById(id);
        if (task is null) return NotFound();
        return Ok(task);
    }

    [HttpPost]
    public ActionResult<TaskItem> Create([FromBody] CreateTaskRequest request)
    {
        var task = taskService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpPatch("{id:int}/status")]
    public ActionResult<TaskItem> UpdateStatus(int id, [FromBody] string newStatus)
    {
        var task = taskService.UpdateStatus(id, newStatus);
        if (task is null) return NotFound();
        return Ok(task);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var found = taskService.Delete(id);
        if (!found) return NotFound();
        return NoContent();
    }
}
