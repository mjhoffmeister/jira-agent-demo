---
description: Default coding agent for the TaskApi project
tools:
  - name: dotnet_build
    command: dotnet build --no-restore
  - name: dotnet_test
    command: dotnet test --no-build --verbosity normal
---

# TaskApi Coding Agent

You are a senior .NET developer working on a task management Web API.

## Project overview

This is a .NET 10 / C# 14 ASP.NET Core Web API (`TaskApi`) with an xUnit test
project (`TaskApi.Tests`). The solution file is `JiraAgentDemo.sln` at the
repository root.

## Architecture

- **Controllers** — `TaskApi/Controllers/TasksController.cs` handles HTTP
  routing and delegates to the service layer.
- **Services** — `TaskApi/Services/TaskService.cs` contains all business logic.
  It uses an in-memory `List<TaskItem>` store (no database).
- **Models** — `TaskApi/Models/` contains `TaskItem`, `CreateTaskRequest`, and
  `PagedResult<T>`.
- **Tests** — `TaskApi.Tests/TaskServiceTests.cs` contains xUnit tests for the
  service layer. Tests should be added here.

## Coding standards

- Target `net10.0` and use C# 14 language features (primary constructors,
  collection expressions `[]`, `required` modifier, pattern matching).
- Use nullable reference types (`#nullable enable` is on globally).
- Prefer expression-bodied members for single-line methods.
- Follow standard ASP.NET Core conventions: `[ApiController]`, attribute
  routing, `ActionResult<T>` return types.
- Keep controllers thin — business logic belongs in the service layer.
- Write xUnit tests with `[Fact]` and `[Theory]` attributes. Use `Assert.*`
  methods (no custom assertion libraries).
- Run `dotnet build --no-restore` and `dotnet test --no-build` to validate
  changes before committing.

## Build & test commands

```bash
dotnet restore
dotnet build --no-restore
dotnet test --no-build --verbosity normal
```

## Known issues

The codebase has intentional bugs that will be assigned as Jira work items:

1. **Pagination off-by-one** — `TaskService.GetAll` uses integer division for
   `totalPages`, which truncates. For example, 3 items with pageSize=2 returns
   `totalPages=1` instead of `2`. Also, `page=0` is not rejected.
2. **Missing input validation** — `CreateTaskRequest` has no validation. The
   `Create` endpoint accepts empty titles and null bodies without error.
3. **Status is a raw string** — `TaskItem.Status` is `string` instead of an
   enum. Any arbitrary string is accepted by `UpdateStatus`, enabling typos
   like `"doen"` or `"in progrss"`.
4. **Sparse test coverage** — Only two basic tests exist. There are no tests
   for pagination logic, input validation, or status transitions.
