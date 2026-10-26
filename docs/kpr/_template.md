---
status: "Current lifecycle state of the defect handling (open, workaround, fix-planned, wont-fix, or fixed)."
date: "Date the status was last updated, in YYYY-MM-DD format."
severity: "Impact of the defect (low, medium, or high)."
affected: "Affected module(s), service(s), or tool(s)."
issue: "Related GitHub issue, e.g. wolfspelz/galdev#123 (optional)."
decision-makers: "People who authorized and signed off on the handling decision."
---

# Short title naming the defect

## Actual Behavior

Describe what actually happens, including the steps or conditions to reproduce it.

## Expected Behavior

Describe what should happen instead.

## Root Cause

Describe the underlying cause if known; "Unknown." is a valid entry — state what was ruled out.

## Considered Handling Options

* Short titles of the handling options evaluated (fix properly by …, workaround: …, leave as is, …).

## Decision

Chosen handling: "<option title>", because <justification — mandatory, especially for wont-fix and "leave as is">.

### Consequences

* Good, because <positive effect of the chosen handling>.
* Bad, because <negative effect of the chosen handling>.

## Workaround

What users, developers, or coding agents must do (or must not do) while the defect exists,
and the workaround's limitations.
Write "None." if there is nothing to do.

## More Information

Related tickets, KPRs, ADRs, the fixing PR once fixed, and review triggers for revisiting the decision.
