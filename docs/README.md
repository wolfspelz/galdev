# Documentation

Documentation for Galactic Developments: the web app (`code/galdevweb/`) and the timeline data (`data/`), in three folders:

| Folder | Holds | Spec |
|--------|-------|------|
| [architecture/](architecture/) | architecture documents, one per area of the code | [architecture/README.md](architecture/README.md) |
| [adr/](adr/) | Architectural Decision Records (MADR) | [adr/README.md](adr/README.md) |
| [kpr/](kpr/) | Known Problem Records: known defects and how each is handled | [kpr/README.md](kpr/README.md) |

The rules in this file apply to every markdown file under `docs/`.
A folder README adds only what is special to its folder.
All documents are written in English.

## Density

**Every document under `docs/` is sparse, information-dense, and free of slack.**
Cut words, never facts.

### Keyword-dense headings

Put the real symbol, error string, or command in the heading; readers arrive by grep, not by reading top to bottom.
Front-load the words a reader scans for, in headings and in sentences alike.

### Imperative rules, not narration

Write the rule, not the story of the rule.

- Good: Pages never read the YAML files under `data/` directly; every entry comes from `TimelineIndex`.
- Bad: It was decided that pages should generally not read the YAML files themselves but go via the index.

### Conclusion first

Open every section with its conclusion, details after.
A reader who stops at the first sentence still has the point.

### One idea per sentence

Each sentence carries a single idea, thought, or concept.
Split any sentence that needs an "and" to join two claims.
A run of sentences sharing a subject and differing in one slot is a table — write the table.

### Snippet over description of snippet

Show the code, config, or command instead of describing it.
Four lines of YAML beat a paragraph about the YAML, and they cannot drift from it.

### Banned phrases

Delete these on sight:

- **Meta-narration** — "This document describes…", "In the following…", "As mentioned above…", "Note that…".
  The heading and the structure already say it.
- **Heading restatement** — a first sentence that repeats the heading above it.
- **Non-discriminating adjectives** — robust, flexible, powerful, simple, seamless.
  An adjective that contrasts with no alternative carries nothing.
- **Hedges without a condition** — "generally", "typically".
  Either the statement always holds, or name the exception; the exception is the information.
- **Nominalizations** — "performs an initialization of" → "initializes".

### Prune every excess word

Replace a phrase with the verb it circles:
"provides a detailed description of" → "describes", "is able to" → "can",
"in order to" → "to", "at this point in time" → "now".

### No trailing recap

End at the last fact.
A closing summary duplicates the document above it and then goes stale on its own schedule.
Delete `## Summary` sections.

### Never cut facts

Density means fewer words, not fewer facts.

## One sentence per line

**Every markdown file under `docs/` puts each sentence on its own line**, with no wrapping inside a sentence;
the one exception is a sentence past the line limit, which gets semantic line breaks (below).
A paragraph is a run of such lines with no blank line between them; blank lines separate paragraphs as usual.
This keeps diffs honest:
editing one sentence rewrites one line, instead of reflowing the whole paragraph and burying the change in whitespace.
Rendering is unaffected — CommonMark, and therefore GitHub, joins the lines back into one flowing paragraph.

### Semantic line breaks

A sentence whose line would exceed 120 characters is broken further,
at a **semantic boundary** rather than at a width limit:
after a subordinate clause, at a semicolon or colon, at a clause-separating comma,
or before a conjunction (the sembr.org convention).
Each line then holds one coherent thought, and editing one clause still rewrites only one line.
Never break mid-phrase just to satisfy the count; when no semantic boundary offers itself, the line simply stays long.

A boundary must also leave a first line of at least 40 characters.
Where every boundary of a sentence sits below that mark, the sentence stays on one long line as well.

Semantic breaks obey the same exceptions as sentence breaks (see *When not to split*):
a continuation line must never start with anything that would open a block,
and headings and table rows stay on one line however long.
In list items, semantic breaks follow the list-item rule: continuation lines are indented to the item's content column.

### List items

Inside a list item, split too, and indent the continuation lines to the item's content column (two spaces after `- `, three after `1. `), example:

```markdown
- **Some bold short** and a continuation.
  And then comes the next sentence of a list item.
- Another normal first sentence of a list item.
  With its own second sentence.

1. A numbered list item's first sentence.
   A second sentence is indented.
1. Just as an example, a single sentence list item.
```

Sibling items get no blank line between them.

**Never indent prose.**
The only thing that indents in a markdown file is list structure — a continuation line under its item,
or a nested item under its parent.
A paragraph outside a list starts at column one;
four spaces would turn it into a code block, and the markdown inside it would stop rendering.

### When not to split

Leave the text on one line where the split would change what is rendered:

- **When the next sentence would start the line with a block marker.**
  A line beginning with `-`, `*`, `+`, `#`, `>`, a code fence, or `1.` / `1)`
  becomes a list item, heading, quote, or code block
  as soon as it owns a line — inside a list item it becomes a nested one.
  Keep such a sentence on the previous sentence's line.
  Any other number is safe mid-paragraph (`2026. was the year…` stays prose),
  because an ordered list can only interrupt a paragraph when it starts at 1
  — but a number does bite as the first line of a block.
- **Headings and table rows** — one line each, however long.
- **Code fences** — the content is literal.
- **YAML front-matter** and **link reference definitions**.
- **Deliberate hard breaks** (two trailing spaces).

Where a sentence ends is a judgement call, not a regex.
These periods do not end one: `e.g.` `i.e.` `etc.` `cf.` `vs.` `ca.` `approx.` `No.` `Dr.` `Prof.` `Inc.` `Ltd.`;
decimals and grouped figures (`1.5`, `300.000`); dates (`22.03.2024`);
versions, file names and dotted identifiers (`v1.2`, `config.py.`, `docs/README.md.`);
ellipses; initials (`A. Einstein`); URLs; and periods inside quotes or parentheses.
When the boundary is unclear, leave the sentences together; an over-eager split is worse than an unsplit line.

### Existing files

**Do not (yet) reflow existing files to match.**
The convention applies to markdown written or edited from now on;
a file is converted only when its content is being rewritten anyway.
When you do convert one, insert and remove line breaks and nothing else
— merge every break inside a paragraph that is not a sentence boundary, and change no words.

## Naming: a date prefix, not a unique key

Pattern: `YYMMDD-kebab-case-title.md`, placed directly in the document's folder (`architecture/`, `adr/`, `kpr/`).

- `YYMMDD` = two-digit year, month, day of month, zero-padded, of the day the document was created
  — e.g. `261021-climate-tour-quests.md` for 21 October 2026.
  It is a **rough ordering hint only**: files sort chronologically in a directory listing.
- **Duplicate prefixes are allowed and expected.**
  Two documents written on the same day share a prefix; that is fine.
  The **filename (slug) is the unique identifier** and must be unique.
- **Never change the prefix** of an existing document — not when it is revised, and not to resolve a collision.
  Renaming breaks citations and history.
- **Cite documents by slug** (e.g. `261021-climate-tour-quests`), not by bare date, because a bare date can be ambiguous.
- Title is short, descriptive, `kebab-case`.

## For coding agents

1. Read this file, then the README of the target folder, before writing or editing anything under `docs/`.
2. Name new documents `YYMMDD-kebab-case-title.md` — see [Naming](#naming-a-date-prefix-not-a-unique-key).
3. Author new documents in **English**.
4. Write **one sentence per line**, breaking sentences past the line limit at semantic boundaries,
   and do not reflow files you are not otherwise rewriting.
5. Write **densely**: cut words, never facts — see [Density](#density).
   Delete any sentence a reader could re-derive from the code, from git, or from another document.
