// Tool to convert YAML timeline entry Text fields from paragraph-per-line to markdown.
// Converts Text to Markdown (sentence per line, paragraphs separated by empty lines)
// and adds Attention key for problem cases.
// Re-runnable: removes existing Markdown/Attention sections before re-creating.

using System.Text.RegularExpressions;

var scriptDir = AppContext.BaseDirectory;
// Navigate up to repo root: bin/Debug/net8.0 -> text2md -> code -> repo root
var repoRoot = Path.GetFullPath(Path.Combine(scriptDir, "..", "..", "..", "..", ".."));
var dataFolder = Path.Combine(repoRoot, "data");

if (!Directory.Exists(dataFolder))
{
    Console.Error.WriteLine($"Error: data folder not found at {dataFolder}");
    Environment.Exit(1);
}

var yamlFiles = Directory.GetFiles(dataFolder, "*.yaml", SearchOption.AllDirectories)
    .OrderBy(f => f)
    .ToList();

Console.WriteLine($"Converting {yamlFiles.Count} YAML files in {dataFolder}:\n");

int convertedCount = 0;
int skippedCount = 0;
int attentionCount = 0;

foreach (var filepath in yamlFiles)
{
    var relPath = Path.GetRelativePath(repoRoot, filepath);
    var result = ConvertFile(filepath);

    switch (result)
    {
        case 0: // Converted
            Console.WriteLine($"{relPath}: converted");
            convertedCount++;
            break;
        case 1: // ConvertedWithAttention
            Console.WriteLine($"{relPath}: converted (needs attention)");
            convertedCount++;
            attentionCount++;
            break;
        case 2: // Skipped
            skippedCount++;
            break;
    }
}

Console.WriteLine();
Console.WriteLine($"Summary: {convertedCount} converted ({attentionCount} need attention), {skippedCount} skipped (no Text)");

// Returns: 0 = Converted, 1 = ConvertedWithAttention, 2 = Skipped
int ConvertFile(string filepath)
{
    var content = File.ReadAllText(filepath);
    var lines = content.Split('\n').ToList();

    // First, remove any existing Markdown: and Attention: sections
    lines = RemoveExistingSections(lines);

    // Find Text: section
    int textStartLine = -1;
    int textEndLine = -1;
    int textIndent = 0;
    var textParagraphs = new List<string>(); // Each item is a paragraph

    for (int i = 0; i < lines.Count; i++)
    {
        var line = lines[i];

        if (line.StartsWith("Text:"))
        {
            textStartLine = i;
            continue;
        }

        if (textStartLine >= 0 && textEndLine < 0)
        {
            // Check if we're still in the Text block (indented lines or empty)
            if (line.Length > 0 && !char.IsWhiteSpace(line[0]))
            {
                // New top-level key, end of Text block
                textEndLine = i;
            }
            else
            {
                // Determine indent of first text line
                if (textIndent == 0 && line.Trim().Length > 0)
                {
                    textIndent = line.TakeWhile(char.IsWhiteSpace).Count();
                }

                // Extract the text content (remove indentation)
                if (line.Length >= textIndent && line.Trim().Length > 0)
                {
                    var textContent = line.Substring(Math.Min(textIndent, line.Length));
                    textParagraphs.Add(textContent);
                }
            }
        }
    }

    // Handle case where Text is at the end of file
    if (textStartLine >= 0 && textEndLine < 0)
    {
        textEndLine = lines.Count;
    }

    // Skip if no Text section found
    if (textStartLine < 0 || textParagraphs.Count == 0)
    {
        return 2; // Skipped
    }

    // Convert paragraphs to sentences and collect attention items
    var markdownLines = new List<string>();
    var attentionItems = new List<string>();
    bool isFirstParagraph = true;
    bool previousWasListItem = false;

    foreach (var paragraph in textParagraphs)
    {
        bool isListItem = Regex.IsMatch(paragraph.TrimStart(), @"^[-*]\s");
        bool isHeading = paragraph.TrimStart().StartsWith("#");

        // Add empty line between paragraphs (not before the first one)
        // But don't add empty line between consecutive list items (dense list)
        if (!isFirstParagraph)
        {
            bool skipEmptyLine = previousWasListItem && isListItem;
            if (!skipEmptyLine)
            {
                markdownLines.Add(""); // Empty line for paragraph separation
            }
        }
        isFirstParagraph = false;
        previousWasListItem = isListItem;

        // Keep headings as-is
        if (isHeading)
        {
            markdownLines.Add(paragraph);
            continue;
        }

        // Keep list items as-is
        if (isListItem)
        {
            markdownLines.Add(paragraph);
            continue;
        }

        // Split paragraph into sentences
        var (sentences, problems) = SplitIntoSentences(paragraph);
        markdownLines.AddRange(sentences);
        attentionItems.AddRange(problems);
    }

    // Build the Markdown section
    var indent = new string(' ', textIndent > 0 ? textIndent : 2);
    var markdownSection = new List<string> { "Markdown: |" };
    foreach (var line in markdownLines)
    {
        if (string.IsNullOrEmpty(line))
        {
            markdownSection.Add(""); // Empty line (no indent needed)
        }
        else
        {
            markdownSection.Add(indent + line);
        }
    }

    // Build the Attention section if needed
    var attentionSection = new List<string>();
    if (attentionItems.Count > 0)
    {
        attentionSection.Add("Attention:");
        foreach (var item in attentionItems.Distinct())
        {
            attentionSection.Add($"  - {item}");
        }
    }

    // Insert Markdown and Attention sections after Text
    var newLines = new List<string>();
    for (int i = 0; i < textEndLine; i++)
    {
        newLines.Add(lines[i]);
    }

    // Add Markdown section
    foreach (var line in markdownSection)
    {
        newLines.Add(line);
    }

    // Add Attention section if needed
    foreach (var line in attentionSection)
    {
        newLines.Add(line);
    }

    // Add remaining lines (after Text section, excluding removed Markdown/Attention)
    for (int i = textEndLine; i < lines.Count; i++)
    {
        newLines.Add(lines[i]);
    }

    // Write the file back
    File.WriteAllText(filepath, string.Join('\n', newLines));

    return attentionItems.Count > 0 ? 1 : 0; // 1 = ConvertedWithAttention, 0 = Converted
}

List<string> RemoveExistingSections(List<string> lines)
{
    var result = new List<string>();
    bool inMarkdown = false;
    bool inAttention = false;

    foreach (var line in lines)
    {
        // Check for start of Markdown section
        if (line.StartsWith("Markdown:"))
        {
            inMarkdown = true;
            inAttention = false;
            continue;
        }

        // Check for start of Attention section
        if (line.StartsWith("Attention:"))
        {
            inAttention = true;
            inMarkdown = false;
            continue;
        }

        // If in Markdown or Attention section, check if we've exited
        if (inMarkdown || inAttention)
        {
            // Still in section if line is empty or starts with whitespace (indented content)
            // or for Attention: if it's a list item starting with "  -"
            if (line.Length == 0 || char.IsWhiteSpace(line[0]))
            {
                continue; // Skip this line (part of section being removed)
            }
            else
            {
                // New top-level key, we've exited the section
                inMarkdown = false;
                inAttention = false;
                result.Add(line);
            }
        }
        else
        {
            result.Add(line);
        }
    }

    return result;
}

(List<string> sentences, List<string> problems) SplitIntoSentences(string paragraph)
{
    var sentences = new List<string>();
    var problems = new List<string>();

    // Two categories of special period cases:
    // 1. AUTO-HANDLED: Known patterns that can be handled automatically (no Attention needed)
    // 2. NEEDS ATTENTION: Patterns that are protected from splitting but flagged for review

    var protected_text = paragraph;
    var placeholders = new Dictionary<string, string>();
    int placeholderIndex = 0;

    // =============================================================================
    // AUTO-HANDLED CASES (protected from splitting, no Attention needed)
    // =============================================================================

    // Protect abbreviations - German
    var germanAbbrevPattern = @"\b(d\.h\.|z\.B\.|u\.a\.|v\.a\.|s\.o\.|s\.u\.|o\.ä\.|u\.ä\.|etc\.|ca\.|bzw\.|inkl\.|exkl\.|ggf\.|evtl\.|bzgl\.|usw\.|Dr\.|Prof\.|Nr\.|Mio\.|Mrd\.|vs\.)";
    protected_text = Regex.Replace(protected_text, germanAbbrevPattern, m =>
    {
        var key = $"§ABBR{placeholderIndex++}§";
        placeholders[key] = m.Value;
        return key;
    }, RegexOptions.IgnoreCase);

    // Protect abbreviations - English
    var englishAbbrevPattern = @"\b(i\.e\.|e\.g\.|etc\.|vs\.|Dr\.|Mr\.|Mrs\.|Ms\.|Prof\.|Inc\.|Ltd\.|Jr\.|Sr\.)";
    protected_text = Regex.Replace(protected_text, englishAbbrevPattern, m =>
    {
        var key = $"§ABBR{placeholderIndex++}§";
        placeholders[key] = m.Value;
        return key;
    }, RegexOptions.IgnoreCase);

    // Protect decimal numbers (e.g., 1.5, 300.000)
    protected_text = Regex.Replace(protected_text, @"(\d+)\.(\d+)", m =>
    {
        var key = $"§DEC{placeholderIndex++}§";
        placeholders[key] = m.Value;
        return key;
    });

    // Protect dates with periods (e.g., 22.03.2024)
    protected_text = Regex.Replace(protected_text, @"\d+\.\d+\.\d+", m =>
    {
        var key = $"§DATE{placeholderIndex++}§";
        placeholders[key] = m.Value;
        return key;
    });

    // Protect URLs
    protected_text = Regex.Replace(protected_text, @"https?://[^\s]+", m =>
    {
        var key = $"§URL{placeholderIndex++}§";
        placeholders[key] = m.Value;
        return key;
    });

    // Protect ellipsis (..., …)
    protected_text = Regex.Replace(protected_text, @"\.\.\.+|…", m =>
    {
        var key = $"§ELL{placeholderIndex++}§";
        placeholders[key] = m.Value;
        return key;
    });

    // Protect ordinal numbers in German (e.g., "22. Jahrhundert")
    protected_text = Regex.Replace(protected_text, @"(\d+)\.\s+([a-zäöü])", m =>
    {
        var key = $"§ORD{placeholderIndex++}§";
        placeholders[key] = m.Groups[1].Value + ".";
        return key + " " + m.Groups[2].Value;
    }, RegexOptions.IgnoreCase);

    // Protect initials (e.g., "A. Einstein")
    protected_text = Regex.Replace(protected_text, @"\b([A-ZÄÖÜ])\.(\s*)([A-ZÄÖÜ])", m =>
    {
        var key = $"§INIT{placeholderIndex++}§";
        placeholders[key] = m.Groups[1].Value + ".";
        return key + m.Groups[2].Value + m.Groups[3].Value;
    });

    // =============================================================================
    // AUTO-HANDLED: Quoted text and parentheses with periods
    // These are protected from splitting but do not need human review
    // =============================================================================

    // Protect quoted text with periods INSIDE the quotes (dialog lines)
    var quotePattern = "([\"\u201E\u201C])([^\"\u201E\u201C\u201D\u201F]*)([\"\u201D\u201C\u201F])";
    protected_text = Regex.Replace(protected_text, quotePattern, m =>
    {
        var innerText = m.Groups[2].Value;
        if (innerText.Contains('.'))
        {
            var key = $"§QUOTE{placeholderIndex++}§";
            placeholders[key] = m.Value;
            return key;
        }
        return m.Value;
    });

    // Handle parentheses with periods
    // Case 1: Complete sentence in parentheses like "(This is a sentence.)" - allow normal split after )
    // Case 2: Period inside but not ending like "(e.g. example)" - just protect from splitting
    var parenPattern = @"\([^)]*\.[^)]*\)";
    protected_text = Regex.Replace(protected_text, parenPattern, m =>
    {
        // Check if it's a complete sentence in parentheses (ends with period before closing paren)
        if (Regex.IsMatch(m.Value, @"\.\)$"))
        {
            // Complete sentence in parentheses - protect the internal period but allow split after )
            // Replace internal periods with placeholder, keep the closing .) intact for splitting
            var inner = m.Value.Substring(1, m.Value.Length - 3); // content without ( and .)
            var key = $"§PARENSENT{placeholderIndex++}§";
            placeholders[key] = inner;
            return "(" + key + ".)";
        }
        else
        {
            // Period inside parentheses but not a complete sentence - just protect
            var key = $"§PAREN{placeholderIndex++}§";
            placeholders[key] = m.Value;
            return key;
        }
    });

    // Step 2: Split by sentence-ending punctuation followed by space
    // Matches: ". " or ".) " or "! " or "!) " or "? " or "?) "
    var rawSentences = Regex.Split(protected_text, @"(?<=[.!?]\)?)\s+");

    // Step 3: Restore placeholders and collect sentences
    foreach (var raw in rawSentences)
    {
        if (string.IsNullOrWhiteSpace(raw)) continue;

        var restored = raw;
        foreach (var kv in placeholders)
        {
            restored = restored.Replace(kv.Key, kv.Value);
        }
        sentences.Add(restored.Trim());
    }

    return (sentences, problems);
}

