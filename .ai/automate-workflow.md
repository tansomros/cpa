# Automated Workflow (Requirements → Done)

This file is for an AI agent (e.g. Claude Code) only — it is not a human-facing runbook. Humans should read [dev-workflow.md](dev-workflow.md) §3.2 instead, which describes the same delivery flow as a manual, human-triggered hybrid process. This file describes the **automated variant**: the same 7 stages, the same rules, the same per-stage templates — just chained together by the AI instead of re-triggered by a human copy-pasting context before every stage.

This file does not redefine any convention. It orchestrates:

- **[prompt-templates.md](prompt-templates.md)** — the actual content/scope of each of the 6 implementation stages (Domain entity+EF, CQRS feature, Domain unit test, Functional test, Frontend, e2e). This file never restates what those templates say — it only sequences when each one runs and what gate sits before/after it.
- **The standing project rules** ([known-issues.md](known-issues.md), [coding-rules.md](coding-rules.md), [ai-agent-guide.md](ai-agent-guide.md), [domain.md](domain.md), and the shared preamble in [prompt-templates.md](prompt-templates.md)) — every MUST/MUST NOT rule, the Non-negotiable money-ledger/concurrency/layered-validation rules, and the "stay within scope" instruction apply unchanged at every stage below. This file adds sequencing and approval gates on top; it does not loosen anything those files require.
- **[requirements/README.md](../requirements/README.md)** — the requirement doc template and its status lifecycle (ร่าง → พร้อมพัฒนา → กำลังพัฒนา → เสร็จสิ้น). This file is the one place that says exactly who (human or AI) is allowed to move a requirement between those statuses — see §5 below.

There are two independent entry points. Only stage 1 is new; stages 2–7 already existed as manual templates.

---

## 1. Stage 1 — Capturing a requirement from a team discussion

**Trigger**: a human asks, in natural language, to turn a discussion into a requirement — e.g. "please write this up as a requirement for {feature}", after a live back-and-forth in this chat, a pasted meeting transcript, or pasted notes. There is no dedicated command for this in the repo; treat any such request as this stage. Do **not** proactively offer to draft a requirement from an unrelated conversation — only act when explicitly asked.

You have no ability to listen to a live meeting yourself (no microphone/audio-capture access) — the input here is always text the team has already produced or is typing live. See [dev-workflow.md](dev-workflow.md) §3.3 for the team's recommended way to get there (recording the meeting + NotebookLM cleanup, then pasting the result here) — that's a human-side prep step, not something this file or the AI needs to do.

**What to do:**

1. Determine the feature name and bounded context. Classify the context using the context folder list in [`requirements/README.md`](../requirements/README.md) (`Master`/`Patient`/`MTM`/`LabResults`/`Settings`/`Security`). If it's genuinely ambiguous or spans multiple contexts, ask the team — don't guess (this mirrors what `requirements/README.md` already tells a human writer to do in the same situation).
2. Extract everything the discussion actually covered into the exact 14 sections of [`_template.md`](../requirements/_template.md). Do not invent a different structure, add sections, or reorder them.
3. Only write what was actually said. Where the discussion left something unresolved, write `ไม่แน่ใจ — ต้องคุยกับทีมก่อน` in that section (the exact phrase `requirements/README.md` already tells human writers to use for the same situation) — do not fabricate plausible-sounding content to fill a gap.
4. **§7 "Money Impact" must always get a real answer, never a guess.** The template itself says this must never be skipped. If the discussion didn't clearly settle whether the feature touches money (and therefore a ledger), ask the team directly before writing the file — this field decides whether the Non-negotiable ledger rule applies downstream, so guessing wrong here corrupts every later stage.
5. Cross-check any Thai business term used against [domain.md](domain.md). If a term isn't defined there, add a definition under §3 (Domain terms) and tell the team `domain.md` may need a matching entry — don't silently invent a new term that later code will use inconsistently.
6. **Always write the status field as `ร่าง` (draft). Never write `พร้อมพัฒนา` or any later status.** This is the pipeline's main human checkpoint — see the ownership table in §5.
7. File path and naming follow `requirements/README.md` exactly: `requirements/{Context}/{feature-name-in-english-kebab-case}.md`, no numeric prefix.
8. Reply to the team with: a short summary of what was captured, an explicit list of open questions/assumptions, and an explicit reminder that a human must review the file and flip its status before implementation can start.
9. If the team keeps discussing the same feature afterward, edit the same file in place — never create a second file for the same feature.

---

## 2. The implementation trigger and its readiness gate

**Trigger**: "please implement @requirements/{context}/{feature}.md" (or equivalent — pointing at a specific requirement file and asking for it to be built).

**Before touching any code, check:**

1. **Status must be `พร้อมพัฒนา` or later.** If it's still `ร่าง`, stop and say so — explain what's still needed, don't proceed "just this once" even if the file looks complete to you. Only a human sets `พร้อมพัฒนา` (§5).
2. **No unfilled template placeholders remain** (literal `{กรอกที่นี่}` text) in a section that's actually relevant to this feature, and §7's checkbox is answered unambiguously (exactly one of the two boxes checked, not both/neither). If either is true, stop and ask rather than guessing what was meant.
3. Do the standing reads: `known-issues.md`, `coding-rules.md`, `ai-agent-guide.md`, and `domain.md` if the feature touches pharmacy/patient/MTM vocabulary. This is unchanged from the manual flow — this file doesn't repeat those rules, it just confirms they still apply here.
4. Once you actually begin stage 2 below, set the requirement's status to `กำลังพัฒนา`.

---

## 3. The plan → approve → execute → verify loop

This is the mechanism that replaces "a human pastes the next template into a new session" — the AI drives it, but a human still approves before every stage's code is written.

1. **Master plan (once, at the start).** Using Plan Mode (`EnterPlanMode`/`ExitPlanMode`), present a single overview plan spanning all remaining stages: bounded context, entity/fields, exactly which commands/queries are in scope per the requirement's §2 (In scope) — do not plan Update/Delete/etc. speculatively if the requirement doesn't ask for them — test coverage, frontend pages, and the e2e flow. This gives the team a chance to correct a misreading of the requirement before any file exists. Wait for approval.
2. **Per-stage plan (every stage, even after the master plan is approved).** Immediately before writing any code for a given stage, present a short, stage-scoped plan: which files this specific stage touches and any decision specific to it (e.g. "this stage adds Create/Get/GetList only, matching the requirement's in-scope list"). Wait for explicit approval before proceeding. This is what catches drift between the master plan and what's actually true once you're inside a stage (a field needing a different type than assumed, a rule that only becomes obvious once you're looking at the entity, etc.).
   - Default behavior is to gate **every** stage this way. A team can explicitly say "go ahead through the rest" to collapse the remaining gates into one approval — treat that as an explicit, one-time opt-in, not a standing default.
3. **Execute** the stage exactly as scoped by [prompt-templates.md](prompt-templates.md)'s matching template (see the mapping table in §4) — same rules, same Do/Don't lists, same file locations as the manual flow.
4. **Verify** using that stage's own command, exactly as specified in the template (see §4's table). Report the actual result — pass or fail — never report a stage as done without having actually run its verification command. This is the existing "report the results, don't just claim success" rule from the shared preamble in [prompt-templates.md](prompt-templates.md), applied per stage instead of once at the end.
5. **A failing verification blocks progress.** Fix and re-verify before presenting the stage as complete or asking to move on — never advance to the next stage on top of a known-broken one.
6. Only after a stage's verification passes and the team has seen the result do you move to the per-stage plan for the next stage.

**Commit automation: none.** This workflow never runs `git commit`/`git push`. It stops at "verified and approved"; the team commits at their own cadence per [dev-workflow.md](dev-workflow.md) §9, exactly as today. Do not treat this file as standing authorization to commit — it isn't.

---

## 4. Stage 2–7 mapping (unchanged from the manual flow)

| Stage | Template in prompt-templates.md | Verification command |
| --- | --- | --- |
| 2 — Domain entity + EF config | §1 "Domain entity + EF configuration" | `dotnet build CPA.sln` |
| 3 — CQRS feature (Application + API) | §2 "CQRS feature (Application + API)" | `dotnet build CPA.sln` |
| 4 — Domain unit tests | §3 "Domain unit test" | `dotnet test tests/Domain.UnitTests/` |
| 5 — Application functional tests | §4 "Application functional test" | `dotnet test tests/Application.FunctionalTests/` |
| 6 — Frontend (Vue) | §5 "Frontend feature (Vue)" | `pnpm run lint && pnpm run build` (from `src/vuewebui`) |
| 7 — Playwright e2e spec | §6 "Playwright e2e spec" | `pnpm test:e2e --grep "@{feature}"` |

Skip stage 6 and/or 7 only when the requirement's own §2 (Out of scope) or §9 (UI/Frontend) says no frontend/e2e coverage is wanted this round — say so explicitly when skipping, don't just quietly omit them.

**Resumability**: if asked to implement a requirement that's already `กำลังพัฒนา` (a later session continuing earlier work — `requirements/README.md` itself notes one requirement often spans multiple passes), check what already exists (the entity file, the CQRS folder, existing tests, existing frontend pages) and resume from the first incomplete stage. Don't restart finished stages, and don't re-ask approval for stages already done and verified in a prior session.

---

## 5. Status ownership — who can set what

This is the safety boundary of the whole pipeline. Get this wrong and the pipeline stops being supervised.

| Status | Meaning | Who sets it |
| --- | --- | --- |
| ร่าง | Draft, not reviewed | AI (stage 1 always writes this), or a human |
| พร้อมพัฒนา | Reviewed, ready to implement | **Human only — the AI must never set this.** |
| กำลังพัฒนา | Implementation started | AI, once stage 2 actually begins (or a human) |
| เสร็จสิ้น | All stages done, verified, and accepted | AI, only after stage 7's verification passes **and** the team has given final sign-off on the result — never on verification passing alone |

---

## 6. What this AI must never do

- Never write or change a requirement's status to `พร้อมพัฒนา` — that transition is exclusively a human decision.
- Never skip a stage's verification command, or report a stage as done without having actually run it.
- Never proceed to the next stage on top of a failing verification.
- Never guess §7 (Money Impact) when it wasn't clearly settled in the discussion — ask instead.
- Never run `git commit`/`git push` as part of this workflow.
- Never build a command/query/page that the requirement's §2 (Out of scope) excludes, even if it seems like an obvious next step — scope comes from the requirement file, not from what would be convenient to add while already in the code.
