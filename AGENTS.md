# AGENTS.md

Instructions for coding agents working in this repo.

## Overview

Galactic Developments — a sci-fi timeline of humanity's future from the mid 21st century to 3365+.
Website: www.galactic-developments.de

The repo has two parts:
- **Code**: ASP.NET Core 8 Razor Pages web app in `code/galdevweb/`
- **Data**: lots of YAML timeline entries in `data/de/` (German) and `data/en/` (English), indexed by `data/data.yaml`

## Commits

- NEVER commit or push on your own — only when explicitly asked to ("commit", "amend", "push", ...).
  After making changes, leave them in the working tree so they can be reviewed in the diff view first.
- Do not ask whether to commit or push; never offer it as a next step. The user says when to commit or push.
- Do NOT add "Co-Authored-By: Claude" to commit messages
- Keep commit messages short: max 2 lines

## Documentation

- Developer documentation lives in `docs/`; read [docs/README.md](docs/README.md) before writing or editing anything there.
