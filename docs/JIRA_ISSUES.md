# Jira Issue Templates for Demo

Create these work items in the Jira `DEMO` project before the demo. Each is
written following the **WRAP** methodology (Write effective issues, Refine
instructions, Atomic tasks, Pair with the coding agent).

---

## Issue 1 — Fix pagination off-by-one error

**Title:** Fix off-by-one error in task pagination — `TaskService.GetAll`

**Description:**

The `GetAll` method in `TaskApi/Services/TaskService.cs` calculates `totalPages`
using integer division (`totalCount / pageSize`), which truncates the result.

**Current behavior:** 3 items with `pageSize=2` returns `totalPages=1`.
**Expected behavior:** Should return `totalPages=2`.

Fix the calculation to use ceiling division:
```csharp
var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
```

Also reject `page < 1` by returning an empty result or throwing
`ArgumentOutOfRangeException`.

Add xUnit tests in `TaskApi.Tests/TaskServiceTests.cs` to cover:
- Exact page boundary (e.g., 4 items, pageSize=2 → totalPages=2)
- Partial last page (e.g., 5 items, pageSize=2 → totalPages=3)
- `page=0` is rejected or handled gracefully

Run `dotnet test` to verify.

**Labels:** `agent-ready`, `bug`

---

## Issue 2 — Add input validation to CreateTaskRequest

**Title:** Add input validation to POST /api/tasks — require non-empty title

**Description:**

The `CreateTaskRequest` model in `TaskApi/Models/CreateTaskRequest.cs` has no
validation. The `POST /api/tasks` endpoint currently accepts empty or null
titles without returning an error.

Add the following validation:
- `Title` is required and must be between 1 and 200 characters. Use
  `[Required]` and `[StringLength(200, MinimumLength = 1)]` attributes.
- `Description` is optional but should be limited to 2000 characters if
  provided. Use `[StringLength(2000)]`.
- Return `400 Bad Request` with validation details when constraints are
  violated.

The controller already uses `[ApiController]` which enables automatic model
validation — adding the attributes to the request model should be sufficient.

Add xUnit tests in `TaskApi.Tests/TaskServiceTests.cs` (or a new test file)
that verify:
- Creating a task with an empty title returns 400
- Creating a task with a valid title succeeds
- Creating a task with a title exceeding 200 characters returns 400

Run `dotnet test` to verify.

**Labels:** `agent-ready`, `enhancement`

---

## Issue 3 — Replace raw status string with enum

**Title:** Replace string-based task status with a `TaskStatus` enum

**Description:**

`TaskItem.Status` in `TaskApi/Models/TaskItem.cs` is currently a `string`.
This allows any arbitrary value (e.g., `"banana"`, `"doen"`, `"in progrss"`).

Create a `TaskStatus` enum in `TaskApi/Models/TaskStatus.cs`:
```csharp
namespace TaskApi.Models;

public enum TaskStatus
{
    Todo,
    InProgress,
    Done
}
```

Update `TaskItem.Status` to use `TaskStatus` instead of `string`. Default
value should be `TaskStatus.Todo`.

Update `TaskService.UpdateStatus` to accept `TaskStatus` instead of `string`.

Update `TasksController.UpdateStatus` accordingly.

The JSON serializer should accept the string names (`"Todo"`,
`"InProgress"`, `"Done"`) — add `[JsonConverter(typeof(JsonStringEnumConverter))]`
to the enum or configure it globally in `Program.cs`.

Add xUnit tests verifying:
- Deserialization of valid status values
- The default status of a new task is `TaskStatus.Todo`

Run `dotnet test` to verify.

**Labels:** `agent-ready`, `enhancement`

---

## Issue 4 — Add unit tests for task service

**Title:** Add comprehensive unit tests for `TaskService`

**Description:**

`TaskApi.Tests/TaskServiceTests.cs` currently has only 2 basic tests. Add
comprehensive coverage for the service layer.

Required test cases:

**Create:**
- Creating multiple tasks assigns sequential IDs
- Created task has `CreatedAt` set to approximately now
- Created task has `CompletedAt` as null

**GetAll (pagination):**
- Empty list returns 0 items and 0 total pages
- Single page of results
- Multiple pages — verify correct items on each page
- Last page with fewer items than page size

**UpdateStatus:**
- Updating status of an existing task returns the updated task
- Updating to "done" sets `CompletedAt`
- Updating a non-existent task returns null

**Delete:**
- Deleting an existing task returns true and removes it
- Deleting a non-existent task returns false
- Deleted task is no longer returned by `GetById`

Run `dotnet test` to verify all tests pass.

**Labels:** `agent-ready`, `tests`
