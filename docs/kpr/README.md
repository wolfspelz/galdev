# Known Problem Records

KPRs in this folder capture known defects and the **decision how each one is handled**:
the observed vs. expected behavior, the root cause (if known), the handling options considered,
the chosen handling, and its consequences.
Every handling decision — including "won't fix" and "leave as is" — must have a documented reason.

The format is our own adaptation of [MADR](https://adr.github.io/madr/) (see `docs/adr/`),
crossed with the classic defect-report fields (actual/expected behavior)
and the ITIL Known Error Record (root cause, workaround, status).
See `_template.md` for the skeleton; file naming and writing conventions are in [docs/README.md](../README.md).

KPRs serve two audiences:

- **Coding agents** — a reference of defects that are known and deliberately handled.
  An agent that stumbles over a documented defect must follow the recorded decision
  (e.g. not "fix" a `wont-fix` in passing, apply the documented workaround) instead of improvising.
- **Developers** — a task list: records with status `open` or `fix-planned` are outstanding work.

This README is the spec for producing a new KPR.

## 1. Decide whether a KPR is needed

Create one if **any** of these is true; the defect:

- is known but will **not** be fixed immediately (won't fix, deferred fix, or workaround in place),
- requires a workaround that users, developers, or coding agents must know about,
- keeps being rediscovered — in reviews, debugging sessions, or by coding agents —
  and each rediscovery costs re-analysis,
- was consciously accepted as a limitation,
  and that decision would otherwise only live in chat history or a closed ticket.

Skip the KPR if:

- the defect is fixed right away in the same PR — the ticket and the fixing commit are documentation enough, or
- it is a feature request or design shortcoming with no wrong observable behavior
  — that is a ticket, or an ADR if it forces a decision, or
- it is technical debt without observable misbehavior — track it as debt, not as a defect.

A KPR records a **decision about a defect**, not the defect report itself.
The GitHub issue stays the place for discussion and reproduction details; the KPR condenses the outcome.

## 2. Front-matter schema

Every KPR starts with this YAML front-matter:

| Field             | Required | Type         | Notes                                                                      |
|-------------------|----------|--------------|-----------------------------------------------------------------------------|
| `status`          | yes      | enum         | `open`, `workaround`, `fix-planned`, `wont-fix`, `fixed`                   |
| `date`            | yes      | `YYYY-MM-DD` | the date `status` last changed                                             |
| `severity`        | yes      | enum         | `low`, `medium`, `high`                                                    |
| `affected`        | yes      | string       | affected module(s), service(s), or tool(s)                                 |
| `issue`           | optional | string       | related GitHub issue, e.g. `wolfspelz/galdev#123`                                             |
| `decision-makers` | yes      | string       | comma-separated names of people who signed off on the handling decision    |

Status meanings:

- `open` — defect confirmed, handling not decided yet (default for a new KPR).
- `workaround` — not fixed; a documented workaround is the agreed handling.
- `fix-planned` — a proper fix is committed to (link the ticket in `issue`).
- `wont-fix` — deliberately left as is; the Decision section must say why.
- `fixed` — resolved; the record stays as history (note the fixing PR/commit under More Information).

## 3. Body sections

Use these section headings, in this order.
Copy `_template.md` and fill in each:

1. `# <short title>` — names the defect in one line.
2. `## Actual Behavior` — what happens, including the steps or conditions to reproduce it.
   Precise enough that a reader can recognize the defect when they hit it.
3. `## Expected Behavior` — what should happen instead.
   The gap between this and the previous section *is* the defect.
4. `## Root Cause` — the underlying cause, if known.
   `Unknown.` is a valid entry — say what was ruled out.
5. `## Considered Handling Options` — bulleted short titles of the options
   (fix properly by …, workaround: …, leave as is, …).
6. `## Decision` — open with `Chosen handling: "<option title>", because <justification>.`
   Mandatory for **every** status — especially for `wont-fix` and "leave as is".
7. `### Consequences` — bulleted "Good, because …" / "Bad, because …" of the chosen handling.
8. `## Workaround` — exactly what users, developers, or coding agents must do (or must not do) while the defect exists,
   and the workaround's limitations.
   Write `None.` if there is nothing to do.
9. `## More Information` — related tickets, KPRs, ADRs, the fixing PR once `fixed`, review dates.

## 4. Lifecycle transitions

Update both `status` and `date` when a KPR moves between states, and extend the Decision section with the new reasoning.
Never delete a KPR — fixed and won't-fix records remain valuable history.

- `open → workaround` — the team agrees on a workaround instead of a fix.
- `open → fix-planned` — a proper fix is committed to and ticketed.
- `open → wont-fix` — the team decides, with reasons, to leave it as is.
- `workaround | wont-fix → fix-planned` — a deferred defect is reprioritized.
- `any → fixed` — the fix has landed; reference the PR/commit.

A `wont-fix` or `workaround` decision should be revisited when its stated reasoning no longer holds
(e.g. the constraining dependency was replaced) — note a review trigger under More Information where possible.
