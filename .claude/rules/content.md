---
paths:
  - "data/**/*"
---

# Content Structure

## Overview

Timeline entries are YAML files in `data/de/` (German, primary) and `data/en/` (English).
The master index `data/data.yaml` maps entry names to file paths per language and defines topics and sequences.

## Entry File Format

Files are named `YEAR_Name.yaml`. Key fields:

- `Name` — internal identifier
- `Year` — timeline year (2051–3365+)
- `Title` — full title
- `ShortTitle` — abbreviated title
- `Short` — one-line summary
- `Image` — image filename (in same language folder's `images/` dir)
- `Author` — typically "Heiner Wolf"
- `Tags` — list; special tags: `_hidden`, `_noweb`, `_nobook`, `_new`, `_hilite`, `_carousel`
- `Topics` — list from: accident, adventure, ai, aliens, catastrophe, conspiracy, crime, culture, discovery, ecology, economy, epidemic, event, life, luck, people, philosophy, politics, science, spaceflight, statistics, technology, things, upgrades, visitors, war, wonder
- `Text` — plain text body (legacy)
- `Markdown` — markdown body (preferred, rendered on site)

See `data/de_template.yaml` / `data/en_template.yaml` for the full template.

## data.yaml Index

- `languages` — defines per-language paths, image dirs, and topic label translations
- `sequences` — named story arcs (e.g. WiseDragon, Spacedom) with per-language title, summary, and ordered entry list
- `entries` — maps canonical entry names to per-language YAML filenames

## Languages

- German (de-DE) is the primary language with the most complete coverage
- English (en-US) has partial coverage; not all entries are translated
- When adding a new entry, a German version is always required; English is optional
