// Validates that the Text: and Markdown: blocks in a YAML timeline entry produce
// the same content. Compares them at the markdown source level after canonicalizing
// each into an ordered list of items (paragraphs, list items, headings).

using System.Text.RegularExpressions;

var scriptDir = AppContext.BaseDirectory;
// Navigate up to repo root: bin/Debug/net8.0 -> text2md-validate -> code -> repo root
var repoRoot = Path.GetFullPath(Path.Combine(scriptDir, "..", "..", "..", "..", ".."));
var dataFolder = Path.Combine(repoRoot, "data");

if (!Directory.Exists(dataFolder))
{
    Console.Error.WriteLine($"Error: data folder not found at {dataFolder}");
    return 2;
}

List<string> filesToCheck;
if (args.Length == 0)
{
    filesToCheck = Directory.GetFiles(dataFolder, "*.yaml", SearchOption.AllDirectories)
        .OrderBy(f => f)
        .ToList();
}
else if (args.Length == 1)
{
    var arg = args[0];
    string? resolved = null;
    if (File.Exists(arg)) resolved = Path.GetFullPath(arg);
    else if (File.Exists(Path.Combine(repoRoot, arg))) resolved = Path.GetFullPath(Path.Combine(repoRoot, arg));
    if (resolved == null)
    {
        Console.Error.WriteLine($"Error: file not found: {arg}");
        return 2;
    }
    filesToCheck = new List<string> { resolved };
}
else
{
    Console.Error.WriteLine("Usage: text2md-validate [path/to/file.yaml]");
    return 2;
}

int okCount = 0;
int diffCount = 0;
int skippedCount = 0;

foreach (var filepath in filesToCheck)
{
    var relPath = Path.GetRelativePath(repoRoot, filepath).Replace('\\', '/');
    var content = File.ReadAllText(filepath);
    var lines = content.Split('\n').ToList();

    var textBlock = ExtractBlock(lines, "Text");
    var mdBlock = ExtractBlock(lines, "Markdown");

    if (textBlock == null || mdBlock == null)
    {
        skippedCount++;
        continue;
    }

    var textItems = CanonicalizeText(textBlock);
    var mdItems = CanonicalizeMarkdown(mdBlock);

    var issues = Compare(textItems, mdItems);
    if (issues.Count == 0)
    {
        okCount++;
        continue;
    }

    diffCount++;
    Console.WriteLine($"{relPath}:");
    foreach (var issue in issues)
    {
        Console.WriteLine("  " + issue);
    }
    Console.WriteLine();
}

Console.WriteLine($"Summary: {filesToCheck.Count} files, {okCount} OK, {diffCount} with differences, {skippedCount} skipped (no Text or no Markdown)");

return diffCount > 0 ? 1 : 0;


// ---------------------------------------------------------------------------

// Extracts the literal-scalar content of a top-level YAML key (e.g. "Text:" or
// "Markdown:"). Returns content lines with the block's leading indent stripped,
// preserving empty lines. Returns null if the block is not found.
static List<string>? ExtractBlock(List<string> lines, string blockName)
{
    int blockStart = -1;
    int indent = 0;
    var content = new List<string>();

    for (int i = 0; i < lines.Count; i++)
    {
        var line = lines[i];

        if (blockStart < 0)
        {
            if (line.StartsWith(blockName + ":"))
            {
                blockStart = i;
            }
            continue;
        }

        // New top-level key encountered → end of block
        if (line.Length > 0 && !char.IsWhiteSpace(line[0]))
        {
            break;
        }

        if (indent == 0 && line.Trim().Length > 0)
        {
            indent = line.TakeWhile(char.IsWhiteSpace).Count();
        }

        if (line.Trim().Length == 0)
        {
            content.Add("");
        }
        else
        {
            content.Add(line.Substring(Math.Min(indent, line.Length)));
        }
    }

    return blockStart < 0 ? null : content;
}

// Each non-empty line is one item (paragraph/list/heading verbatim).
static List<string> CanonicalizeText(List<string> lines)
{
    var result = new List<string>();
    foreach (var l in lines)
    {
        var n = NormalizeWhitespace(l);
        if (n.Length > 0) result.Add(n);
    }
    return result;
}

// Split on blank lines into groups. A group whose every line begins with a list
// marker becomes one item per line; any other group is joined with a single space.
static List<string> CanonicalizeMarkdown(List<string> lines)
{
    var result = new List<string>();
    var group = new List<string>();

    void Flush()
    {
        if (group.Count == 0) return;
        bool allListItems = group.All(l => Regex.IsMatch(l.TrimStart(), @"^[-*]\s"));
        if (allListItems)
        {
            foreach (var l in group) result.Add(NormalizeWhitespace(l));
        }
        else
        {
            result.Add(NormalizeWhitespace(string.Join(" ", group.Select(l => l.Trim()))));
        }
        group.Clear();
    }

    foreach (var l in lines)
    {
        if (l.Trim().Length == 0) Flush();
        else group.Add(l);
    }
    Flush();

    return result;
}

static string NormalizeWhitespace(string s) => Regex.Replace(s, @"\s+", " ").Trim();

static List<string> Compare(List<string> text, List<string> md)
{
    var issues = new List<string>();
    var placeholderRe = new Regex(@"§\w+\d+§");

    if (text.Count != md.Count)
    {
        issues.Add($"! Length mismatch: Text={text.Count} items, Markdown={md.Count} items");
    }

    int n = Math.Max(text.Count, md.Count);
    for (int i = 0; i < n; i++)
    {
        string? t = i < text.Count ? text[i] : null;
        string? m = i < md.Count ? md[i] : null;

        if (m != null)
        {
            var leak = placeholderRe.Match(m);
            if (leak.Success)
            {
                issues.Add($"[item {i}] PLACEHOLDER LEAK in MD: {leak.Value}");
                issues.Add($"  TEXT: {Snip(t)}");
                issues.Add($"  MD:   {Snip(m)}");
                continue;
            }
        }

        if (t == null)
        {
            issues.Add($"[item {i}] EXTRA in MD: {Snip(m)}");
            continue;
        }
        if (m == null)
        {
            issues.Add($"[item {i}] MISSING in MD: {Snip(t)}");
            continue;
        }
        if (t != m)
        {
            issues.Add($"[item {i}] DIFFERS:");
            var (tWin, mWin) = DiffWindows(t, m);
            issues.Add($"  TEXT: {tWin}");
            issues.Add($"  MD:   {mWin}");
        }
    }

    return issues;
}

static string Snip(string? s, int max = 200)
{
    if (s == null) return "<absent>";
    if (s.Length <= max) return s;
    return s.Substring(0, max) + "…";
}

// Returns aligned snippets that center on the first divergence so the
// differing region is always visible regardless of where it occurs.
static (string, string) DiffWindows(string t, string m, int lead = 40, int trail = 40)
{
    int prefix = 0;
    int min = Math.Min(t.Length, m.Length);
    while (prefix < min && t[prefix] == m[prefix]) prefix++;

    int tSuf = t.Length, mSuf = m.Length;
    while (tSuf > prefix && mSuf > prefix && t[tSuf - 1] == m[mSuf - 1]) { tSuf--; mSuf--; }

    string Build(string s, int sufEnd)
    {
        int start = Math.Max(0, prefix - lead);
        int end = Math.Min(s.Length, sufEnd + trail);
        var head = start > 0 ? "…" : "";
        var tail = end < s.Length ? "…" : "";
        return head + s.Substring(start, end - start) + tail;
    }

    return (Build(t, tSuf), Build(m, mSuf));
}
