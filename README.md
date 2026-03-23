# Jira-to-GitHub Agent Orchestration Demo

A demo showing Jira issues flowing to GitHub where Copilot coding agent
autonomously implements fixes and opens pull requests.

## Project

- **TaskApi** — .NET 10 / C# 14 task management Web API with intentional bugs
- **TaskApi.Tests** — xUnit test project (intentionally sparse)

## Quick start

```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project TaskApi
```

API is available at `https://localhost:5001/api/tasks`.

## Demo paths

### Option A — First-party integration (recommended)

Uses the **GitHub Copilot coding agent for Jira** integration (public preview,
March 2026). Requires Jira Cloud Standard or higher with Rovo enabled.

1. Install the [GitHub Copilot for Jira](https://marketplace.atlassian.com/apps/1582455624)
   app from Atlassian Marketplace.
2. Install the companion [GitHub App](https://github.com/marketplace/github-copilot-for-jira).
3. Connect your GitHub org and configure repo access.
4. Create Jira issues from the templates in [`docs/JIRA_ISSUES.md`](docs/JIRA_ISSUES.md).
5. Assign `GitHub Copilot` to a work item — Copilot reads the Jira context,
   opens a draft PR, and posts updates back to Jira.

### Option B — Custom sync workflows (fallback)

Uses the Jira free tier with GitHub Actions workflows that poll Jira and sync
issues/status bidirectionally. No Rovo required.

- [`.github/workflows/jira-to-github.yml`](.github/workflows/jira-to-github.yml) —
  Polls Jira for `agent-ready` issues, creates GitHub Issues, assigns Copilot.
- [`.github/workflows/github-to-jira.yml`](.github/workflows/github-to-jira.yml) —
  Updates Jira status and posts comments when PRs are opened/merged.

Required secrets: `JIRA_BASE_URL`, `JIRA_USER_EMAIL`, `JIRA_API_TOKEN`.

## Intentional bugs

The codebase has 4 scoped, fixable bugs — each mapped to a Jira issue template:

| # | Bug | File | Jira issue |
|---|-----|------|------------|
| 1 | Pagination off-by-one | `TaskApi/Services/TaskService.cs` | Fix pagination |
| 2 | No input validation | `TaskApi/Models/CreateTaskRequest.cs` | Add validation |
| 3 | Status is raw string | `TaskApi/Models/TaskItem.cs` | Replace with enum |
| 4 | Sparse test coverage | `TaskApi.Tests/TaskServiceTests.cs` | Add unit tests |

## Agent configuration

[`.github/agents/AGENTS.md`](.github/agents/AGENTS.md) provides the coding agent
with project context, coding standards, and build/test commands.

## Demo runbook

See [`docs/DEMO_RUNBOOK.md`](docs/DEMO_RUNBOOK.md) for the narrated walkthrough.
