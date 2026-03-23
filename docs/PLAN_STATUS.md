# Jira-to-GitHub Agent Orchestration Demo — Plan Status

**Last updated:** 2026-03-23

---

## Completed

- [x] **Demo app created** — .NET 10 / C# 14 Web API (`TaskApi`) in `c:\source\jira-agent-demo` with 4 intentional bugs (pagination off-by-one, missing input validation, raw string status, sparse tests)
- [x] **AGENTS.md** — `AGENTS.md` (repo root) with project context, coding standards, build/test commands, and tool definitions for `dotnet build`/`dotnet test`
- [x] **CI workflow** — `.github/workflows/build.yml` (build + test on PR/push to main, SHA-pinned actions)
- [x] **Option B fallback workflows** — `jira-to-github.yml` (polls Jira, creates GitHub Issues, assigns `copilot-swe-agent[bot]`) and `github-to-jira.yml` (transitions Jira status on PR open/merge)
- [x] **Jira issue templates** — `docs/JIRA_ISSUES.md` with 4 WRAP-style issues ready to paste into Jira
- [x] **Demo runbook** — `docs/DEMO_RUNBOOK.md` with narrated script, pre-flight checklist, and troubleshooting table
- [x] **Build verified** — `dotnet build` passes, 2/2 tests green

## Key Finding from Research

**GitHub shipped a first-party "Copilot coding agent for Jira" integration** (public preview, March 5, 2026). This eliminates all custom sync workflow code. You assign `GitHub Copilot` directly to a Jira work item, it reads Jira context, opens a draft PR on GitHub, and posts updates back to Jira.

**Catch:** Requires Jira Cloud with **Rovo enabled** (Standard plan or higher — NOT free tier). Free trial of Standard is available.

## Next Steps

### 1. Push to GitHub

```bash
cd c:\source\jira-agent-demo
git add -A
git commit -m "Initial demo app — .NET 10 task API with intentional bugs"
gh repo create jira-agent-demo --private --source . --push
```

### 2. Enable Copilot coding agent on the repo

- Repo Settings → Copilot → Enable coding agent
- Verify `copilot-swe-agent` appears in assignees list

### 3. Decide Jira path

| Path | Jira Plan | Setup | Complexity |
|------|-----------|-------|------------|
| **Option A (recommended)** | Standard free trial | Install 2 marketplace apps, enable Rovo | Minimal — no custom code |
| **Option B (fallback)** | Free tier ($0) | Configure 3 secrets, workflows already built | Medium — custom sync workflows |

**Option A setup:**
1. Start Jira Standard trial at https://www.atlassian.com/try
2. Enable Rovo: Atlassian Admin → AI features → Activate AI for apps + Beta AI features
3. Install [GitHub Copilot for Jira](https://marketplace.atlassian.com/apps/1582455624) (Atlassian Marketplace)
4. Install companion [GitHub App](https://github.com/marketplace/github-copilot-for-jira)
5. Connect GitHub org, configure repo access

**Option B setup:**
1. Create free Jira Cloud account
2. Set GitHub secrets: `JIRA_BASE_URL`, `JIRA_USER_EMAIL`, `JIRA_API_TOKEN`
3. Workflows are already in `.github/workflows/`

### 4. Create Jira issues

Copy the 4 issues from `docs/JIRA_ISSUES.md` into the Jira `DEMO` project. Label each `agent-ready`.

### 5. Pre-stage one issue

Assign one issue to the agent ahead of the demo so there's a PR mid-flight. This avoids dead air during the live walkthrough (agent can take a few minutes).

### 6. Rehearse

Run through `docs/DEMO_RUNBOOK.md` end-to-end at least once. Screen-record as fallback.

### 7. Check for blockers

- "Require signed commits" ruleset rule → blocks coding agent. Add Copilot as bypass actor.
- Actions workflow approval → agent PRs need "Approve and run" by default. Optionally enable auto-approval (repo settings, March 2026 changelog).

## File Tree

```
c:\source\jira-agent-demo\
├── AGENTS.md                       # Coding agent instructions
├── .github/
│   └── workflows/
│       ├── build.yml               # CI: build + test
│       ├── jira-to-github.yml      # Option B: inbound sync
│       └── github-to-jira.yml      # Option B: outbound sync
├── docs/
│   ├── DEMO_RUNBOOK.md             # Narrated demo script
│   ├── JIRA_ISSUES.md              # 4 issue templates for Jira
│   └── PLAN_STATUS.md              # This file
├── TaskApi/
│   ├── Controllers/TasksController.cs
│   ├── Models/
│   │   ├── CreateTaskRequest.cs    # BUG: no validation
│   │   ├── PagedResult.cs
│   │   └── TaskItem.cs             # BUG: string status
│   ├── Services/TaskService.cs     # BUG: pagination off-by-one
│   └── Program.cs
├── TaskApi.Tests/
│   └── TaskServiceTests.cs         # BUG: only 2 tests
├── JiraAgentDemo.slnx
└── README.md
```
