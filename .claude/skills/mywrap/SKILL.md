---
name: mywrap
description: Reformat the Markdown block of the currently open YAML data file so each sentence is on its own line, paragraphs separated by blank lines (git-friendly layout)
---

# Wrap Markdown Block of Open YAML File

Reformat the `Markdown:` block of the currently open YAML file into git-friendly layout: each sentence on its own line, paragraphs separated by a blank line.

## Target

- Use the YAML file the user currently has open in the IDE (see `ide_opened_file` context).
- If no YAML file is open, ask the user which file to process.
- Only the `Markdown: |` block is modified. The `Text:` block and all other fields stay untouched.

## Rules (strict)

- **No text changes.** Only insert/remove line breaks. Do not fix typos, punctuation, spacing, or wording.
- **One sentence per line.** Split at sentence boundaries (`.`, `!`, `?`) followed by whitespace and a capital letter / opening quote / digit.
- **Preserve paragraphs.** Keep blank lines between paragraphs. A single-newline break inside a paragraph that is not a sentence boundary should be merged into the previous sentence with a space.
- **Watch out for false boundaries:** abbreviations (`z. B.`, `ca.`, `Dr.`, `Mio.`, `Mrd.`, `Nr.`, `usw.`, `etc.`, `vs.`, `St.`), ordinals (`1.`, `21.`), decimal numbers (`3.14`), ellipses (`...`), and initials. Do not split at these.
- **Preserve YAML indentation.** The `Markdown: |` block is indented by 2 spaces. Every content line (including blank paragraph separators — use an empty line, not a line with 2 spaces) must keep that indentation so the YAML stays valid.
- **Preserve the trailing blank line** after the block if present.

## Usage

1. Determine the open YAML file from the IDE context.
2. Read the file.
3. Locate the `Markdown: |` block and extract its content (strip the 2-space indent).
4. Apply the rules above to reformat.
5. Re-indent by 2 spaces and write the file back with `Edit` (replace only the old block content with the new one).
6. Report which file was changed and how many paragraphs/sentences resulted.

## Example

Input block (inside the YAML):

```
# Metallklumpen
Auf dem Saturnmond Pandora wird ein radioaktiv strahlender Metallklumpen entdeckt. Die Zusammensetzung und die Art der Aktivierung deuten auf einen künstlichen Ursprung hin.
- Eisen
- Uran
Tatsächlich war das Artefakt schon 2162 von einem Prospektor der Zhu-Republik (Südchina) entdeckt worden. Der Fund war in der Hoffnung auf andere Entdeckungen geheim gehalten worden.
```

Output block (written back):

```
# Metallklumpen

Auf dem Saturnmond Pandora wird ein radioaktiv strahlender Metallklumpen entdeckt.
Die Zusammensetzung und die Art der Aktivierung deuten auf einen künstlichen Ursprung hin.

- Eisen
- Uran

Tatsächlich war das Artefakt schon 2162 von einem Prospektor der Zhu-Republik (Südchina) entdeckt worden.
Der Fund war in der Hoffnung auf andere Entdeckungen geheim gehalten worden.
```
