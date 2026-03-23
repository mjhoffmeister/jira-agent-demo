# Demo Runbook — Jira-to-GitHub Agent Orchestration

Narrated walkthrough (~3 minutes, excluding agent execution time).

---

## Pre-demo checklist

- [ ] Jira Cloud instance is live (Standard free trial or free tier)
- [ ] 4 Jira issues created from [`JIRA_ISSUES.md`](JIRA_ISSUES.md)
- [ ] GitHub repo created and pushed (this repo)
- [ ] AGENTS.md is present at repo root (`AGENTS.md`)
- [ ] Copilot coding agent is enabled for the repo
- [ ] Build & Test workflow passes on `main`
- [ ] **Option A only:** GitHub Copilot for Jira app installed and configured
- [ ] **Option B only:** Secrets configured (`JIRA_BASE_URL`, `JIRA_USER_EMAIL`, `JIRA_API_TOKEN`)
- [ ] One issue pre-staged mid-flow (agent already working) — avoids dead air
- [ ] Screen recording running as backup

---

## Act 1 — Set the Scene (30 seconds)

**Show:** Jira board with 4 issues in "To Do" column.

> "Here's a typical dev team backlog tracked in Jira. We've got bugs, missing
> validation, a refactoring task, and a test coverage gap. The question is:
> what if we could hand these directly to an AI coding agent — without
> leaving Jira?"

---

## Act 2 — Trigger the Agent (30 seconds)

### Option A (first-party)

**Do:** Open a Jira issue. Assign `GitHub Copilot` in the Assignee field.

> "I'm assigning this issue to GitHub Copilot — the first-party integration
> that shipped in March 2026. Copilot reads the issue title, description,
> and comments. No custom code, no glue workflows."

### Option B (custom sync)

**Do:** Show the `jira-to-github.yml` workflow. Trigger manually via
`workflow_dispatch`.

> "Our sync workflow polls Jira for issues labeled 'agent-ready', creates a
> GitHub Issue, and assigns the Copilot coding agent via the REST API."

---

## Act 3 — Agent at Work (60 seconds)

**Show:** (Switch to pre-staged issue that's already mid-execution.)

- **GitHub:** Copilot has created a `copilot/` branch and opened a draft PR.
- **Jira (Option A):** Agent panel shows progress updates.
- **Session logs:** Click into the Copilot session to show it running
  `dotnet build`, `dotnet test`, reading files, making changes.

> "Copilot spins up a secure cloud sandbox, clones the repo, analyzes the
> code, makes changes, then builds and tests. It's running in GitHub Actions
> infrastructure — same compute, same security model."

**Highlight:** Built-in security — CodeQL, secret scanning, dependency audit.

> "Before it finishes, Copilot automatically runs CodeQL for security
> issues, checks for hardcoded secrets, and scans new dependencies against
> the GitHub Advisory Database. No GHAS license required — this is built in."

---

## Act 4 — Review and Merge (30 seconds)

**Show:** The draft PR with code changes.

- Walk through a few changed lines (e.g., the `Math.Ceiling` fix).
- Show that the PR was created on a `copilot/` branch.
- Show the co-author commit attribution (governance trail).

> "The PR is a draft. Copilot can't mark it ready or merge it — a human
> reviews and approves. The commit is co-authored by the developer who
> assigned the issue, so there's a clear audit trail."

**Do:** Approve the PR. Click "Ready for review", then merge.

---

## Act 5 — Results Flow Back (30 seconds)

### Option A

**Show:** Jira issue — agent panel shows completion. PR link is visible.

> "Back in Jira, the agent panel shows the PR was merged. The developer
> never left Jira during triage. Copilot never left GitHub during
> implementation. Each tool does what it's best at."

### Option B

**Show:** Jira issue updated via workflow — status transitioned to "Done",
comment posted with PR link and merge SHA.

---

## Act 6 — Zoom Out (30 seconds)

> "What we just saw works today, in public preview. No custom infrastructure.
> Jira is one connector — GitHub also supports Azure Boards, Linear, Slack,
> and Teams. The pattern is tracker-agnostic."
>
> "For Ascension, this maps directly to the Agentic DevOps narrative: a
> security finding from OX creates an issue, the coding agent picks it up,
> and the fix flows through your existing governance gates — rulesets,
> reusable workflows, required reviews, and build provenance attestation."

---

## Troubleshooting

| Problem | Fix |
|---------|-----|
| Agent not appearing in Jira Assignee list | Check Rovo is enabled + Beta AI features on. Refresh page. |
| Agent not responding after assignment | Check GitHub status page. Verify coding agent access on the repo. |
| `copilot-swe-agent[bot]` can't be assigned (Option B) | Verify Copilot coding agent is enabled in repo settings. |
| "Require signed commits" blocks agent | Add Copilot as a bypass actor in the ruleset. |
| Workflow approval required for agent PRs | Either click "Approve and run" manually, or enable auto-approval in repo settings (March 2026 changelog). |
