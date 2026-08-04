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
- `Markdown` — markdown body, rendered on site

See `data/de_template.yaml` / `data/en_template.yaml` for the full template.

## Formatting the `Markdown` body

Binding whenever the `Markdown:` block is written or edited.

### One sentence per line

No wrapping inside a sentence.
Blank lines separate paragraphs, not sentences.
One edited sentence is then one changed line; rendering is unaffected.

### Splitting inside list items

Split inside list items too, indenting continuation lines to the content column — two spaces after `- `, three after `1. `.
Too little ends the item, too much makes a code block.
Sibling items get no blank line between them.

### When not to split

Never split where the break would change what is rendered:
when the next sentence would begin the line with `-`, `*`, `+`, `#`, `>`, a fence or `1.` / `1)` — other numbers are safe mid-paragraph, but any number becomes a list as the first line of a block; table rows; code; frontmatter; link reference definitions; hard breaks; headings.

Periods that do not end a sentence:
`d.h.` `z.B.` `u.a.` `v.a.` `s.o.` `s.u.` `o.ä.` `u.ä.` `bzw.` `inkl.` `exkl.` `ggf.` `evtl.` `bzgl.` `usw.` `etc.` `ca.` `Dr.` `Prof.` `Nr.` `Mio.` `Mrd.` `St.` `vs.` `i.e.` `e.g.` `Mr.` `Mrs.` `Ms.` `Inc.` `Ltd.` `Jr.` `Sr.`; ordinals (`22. Jahrhundert`); decimals and grouped figures (`1.5`, `300.000`); dates (`22.03.2024`); versions and filenames (`v1.2`, `data.yaml.`); ellipses; initials (`A. Einstein`); URLs; periods inside quotes or parentheses.
Where the boundary is unclear, leave the sentences together — an over-eager split is worse than an unsplit line.

### Semantic line breaks

A sentence whose line would exceed 120 characters is broken further,
at a **semantic boundary** rather than at a width limit:
after a subordinate clause, at a semicolon or colon,
at a clause-separating comma, or before a conjunction.
Each line then holds one coherent thought, and editing one clause still rewrites only one line.
Never break mid-phrase just to satisfy the count;
when no semantic boundary offers itself, the line simply stays long.

Semantic breaks obey the same exceptions as sentence breaks (see [When not to split](#when-not-to-split)):
a continuation line must never start with anything that would open a block,
and headings and table rows stay on one line however long.
In list items, semantic breaks follow the list-item rule:
continuation lines are indented to the item's content column.

### Indentation

The `Markdown:` block is indented two spaces, so a list continuation line sits at two plus two.

```yaml
Markdown: |
  The probe reaches the outer moon in the third year of the mission.
  Its cameras see nothing that was not already in the catalogue.

  - The hull is intact.
    Corrosion is limited to the docking ring, which had been written off anyway.
  - The reactor is cold.
```

Separator lines are truly empty, the trailing blank line stays, and `Text:` keeps its own layout.

### Reflowing existing text

Existing text is not reflowed to match, only converted when it is being rewritten anyway.
Then insert and remove line breaks and nothing else, merging any break inside a paragraph that is not a sentence boundary.

## data.yaml Index

- `languages` — defines per-language paths, image dirs, and topic label translations
- `sequences` — named story arcs (e.g. WiseDragon, Spacedom) with per-language title, summary, and ordered entry list
- `entries` — maps canonical entry names to per-language YAML filenames

## Languages

- German (de-DE) is the primary language with the most complete coverage
- English (en-US) has partial coverage; not all entries are translated
- When adding a new entry, a German version is always required; English is optional
