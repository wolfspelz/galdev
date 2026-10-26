# Architecture Documents

One document per area of the code or the tooling: the design of record that a change in that area is checked against.
Naming and writing conventions are in [docs/README.md](../README.md).

## What a document covers

- The structures and responsibilities of the area, and the data flow between them.
- Keyword-dense headings with the real symbol, error string or command: readers arrive by grep.
- Imperative rules, not narration: what the code must do, not the story of how it came to.
- Design choices only as far as the current architecture needs them; the decision itself goes to an [ADR](../adr/README.md).
- Known defects that stay unfixed go to a [KPR](../kpr/README.md), not into the architecture document.

## Index

| Document | Area |
|----------|------|
| [261026-lego-model-rendering](261026-lego-model-rendering.md) | Rendering Lego model images (blog title images) from the build-instruction viewer, with or without the stand |

Add a row when a document is added.
