# Architectural Decision Records

ADRs in this folder capture architecturally significant decisions:
the context, the options considered, the choice, and the consequences.
They are append-only — an outdated decision is corrected by a new ADR that supersedes the old one,
never by editing history.
We use the [MADR](https://adr.github.io/madr/) format.
See `_template.md` for the skeleton; file naming and writing conventions are in [docs/README.md](../README.md).

This README is the spec for producing a new ADR.

## 1. Decide whether an ADR is needed

Create one if **any** of these is true; the change:

- alters architecture across multiple modules or services,
- introduces, removes, or replaces a major dependency or framework,
- changes a project-wide convention (testing, logging, error handling, concurrency model, …),
- captures a choice the team would otherwise rediscover from chat history months later.

Skip the ADR if:

- the change is local refactoring fully captured by the PR description, or
- the decision has not been made yet — only explored.
  Use a ticket or planning doc and come back when the team commits.

**Do not write an ADR after the change has shipped without prior discussion.**
That reduces the document to rationalization.
Author it at decision time, while alternatives are still on the table;
the ADR PR may land before, alongside, or just after the implementation PR.

**Speculative ADRs are discouraged**, with one exception:
a forward-looking commitment the team has actually made (e.g. "we will adopt X when Y begins next quarter")
warrants an `accepted` ADR even if no code references it yet.

If you are mid-implementation and notice you are quietly making an architectural choice nobody discussed,
stop and open an ADR.

## 2. Front-matter schema

Every ADR starts with this YAML front-matter:

| Field             | Required | Type            | Notes                                                                       |
|-------------------|----------|-----------------|------------------------------------------------------------------------------|
| `status`          | yes      | enum            | `proposed`, `accepted`, `deprecated`, `superseded by <slug>`, `rejected`     |
| `date`            | yes      | `YYYY-MM-DD`    | the date `status` last changed                                              |
| `decision-makers` | yes      | string          | comma-separated names of people who signed off                              |
| `consulted`       | optional | string          | subject-matter experts consulted in two-way communication                   |
| `informed`        | optional | string          | stakeholders kept up to date in one-way communication                       |

Default `status` for a brand-new ADR:

- `proposed` if the team is still debating,
- `accepted` if the team has already committed.

## 3. Body sections (MADR)

Use these section headings, in this order.
Copy `_template.md` and fill in each:

1. `# <short title>` — restates the problem and the chosen solution.
2. `## Context` — the background, situation, and explanations a reader needs to understand the decision.
   Do **not** state the problem here.
3. `## Problem Statement` — set apart in its own section, state the one question to be decided,
   phrased as a question and ideally a single sentence.
   A reader must be able to find that question at a glance without disentangling it from the surrounding explanation.
4. `## Decision Drivers` — bulleted forces, constraints, concerns.
5. `## Considered Options` — bulleted short titles of the options.
6. `## Decision Outcome` — open with `Chosen option: "<title>", because <justification>.`
7. `### Consequences` — bulleted "Good, because …" / "Bad, because …".
8. `### Confirmation` — how compliance is verified (code review, fitness function, automated test).
9. `## Pros and Cons of the Options` — one `### <option>` subsection per option from §5,
   with bulleted Good / Neutral / Bad arguments.
10. `## More Information` — links to related decisions, review dates, further reading.

## 4. Level of abstraction: decisions, not implementation plans

An ADR records a **general architecture decision**, not an implementation plan.
Reduce the use of code-names — class names, function names, file paths.
Rather than naming the symbol from the code, **describe the software structure** it represents and the role it plays.

- Write "the service's configuration object", not `ServiceConfig`.
- Write "a per-customer data bundle mounted read-only into each service",
  not `docker_compose_generator._generate_data_section` / `volumes_from`.
- Write "a model manifest loaded at startup", not `ModelZoo(models.yaml)`.

Why: an ADR must stay valid when the code is renamed, moved, or refactored.
A decision phrased in terms of structures and responsibilities survives that;
one phrased in terms of today's class names becomes stale and misleading.

If a concrete anchor genuinely aids understanding, confine it to `## More Information`,
and keep the decision itself expressed in structural, ubiquitous-language terms.

## 5. Lifecycle transitions

Update both `status` and `date` when an ADR moves between states.
Never delete an ADR — superseded and rejected ones remain valuable history.

- `proposed → accepted` — the team commits.
- `accepted → deprecated` — no longer recommended, no replacement yet.
- `accepted → superseded by <slug>` — replaced by a newer ADR.
- `proposed → rejected` — explicitly turned down.

### Size limits

An ADR is at most three pages.
When one still exceeds three pages after every rule above is applied, do not cut facts to fit.
Take it to the developer and decide together
what splits off into its own document and what the decision genuinely requires.
