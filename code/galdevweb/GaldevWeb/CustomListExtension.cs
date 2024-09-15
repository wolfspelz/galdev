using Markdig;
using Markdig.Syntax;
using Markdig.Renderers;
using Markdig.Helpers;
using Markdig.Parsers;

namespace GaldevWeb;

public class CustomListExtension : IMarkdownExtension
{
    public string Prefix { get; set; } = "";
    public string Suffix { get; set; } = "";

    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        // Explicitly disable the default list parser to prevent conflicts
        var listParser = pipeline.BlockParsers.FindExact<ListBlockParser>();
        if (listParser != null) {
            pipeline.BlockParsers.Remove(listParser);
        }

        // Add the custom list block parser
        pipeline.BlockParsers.AddIfNotAlready(new CustomListBlockParser() { Prefix = Prefix, Suffix = Suffix });
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        // No changes needed to the rendering process
    }
}

public class CustomListBlockParser : BlockParser
{
    public string Prefix { get; set; } = "";
    public string Suffix { get; set; } = "";

    public CustomListBlockParser()
    {
        OpeningCharacters = new[] { '-' };  // Set the dash as an opening character for parsing
    }

    // Public method to ensure it's invoked
    public override BlockState TryOpen(BlockProcessor processor)
    {
        var line = processor.Line.ToString().TrimStart();

        // Check if the line starts with "- " (dash followed by space)
        if (line.StartsWith("- ")) {
            // Prefix the line to mark it as a list item
            var content = line.Substring(2).TrimStart(); // Remove "- " and trim the remaining spaces
            processor.Line = new StringSlice($"{Prefix}{content}{Suffix}");

            // Treat it as a regular paragraph (returning BlockState.None avoids further list logic)
            return BlockState.None;
        }
        // Check if the line starts with a dash followed by any other character
        else if (line.StartsWith("-")) {
            // Prefix the line as a regular paragraph
            processor.Line = new StringSlice(line);

            // Return BlockState.None to treat this as a normal paragraph
            return BlockState.None;
        }

        return BlockState.Continue;
    }

    public override BlockState TryContinue(BlockProcessor processor, Block block)
    {
        // We don't need to continue any list logic, treat it as a regular block
        return BlockState.None;
    }
}
