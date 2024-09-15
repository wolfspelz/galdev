using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace GaldevWeb;

public class DemoteHeadingsExtension : IMarkdownExtension
{
    private readonly int _demoteLevel;

    public DemoteHeadingsExtension(int demoteLevel)
    {
        _demoteLevel = demoteLevel;
    }

    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        // No changes needed to the parsing process
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        // Customize the HTML renderer for headings
        if (renderer is HtmlRenderer htmlRenderer) {
            // Remove the existing heading renderer
            var headingRenderer = htmlRenderer.ObjectRenderers.FindExact<HeadingRenderer>();
            if (headingRenderer != null) {
                htmlRenderer.ObjectRenderers.Remove(headingRenderer);
            }

            // Add the custom heading renderer that demotes headings by the specified level
            htmlRenderer.ObjectRenderers.Add(new DemoteHeadingRenderer(_demoteLevel));
        }
    }
}

public class DemoteHeadingRenderer : HtmlObjectRenderer<HeadingBlock>
{
    private readonly int _demoteLevel;

    public DemoteHeadingRenderer(int demoteLevel)
    {
        _demoteLevel = demoteLevel;
    }

    protected override void Write(HtmlRenderer renderer, HeadingBlock obj)
    {
        // Demote the heading level by the specified number (but keep a minimum level of 1)
        int level = obj.Level + _demoteLevel;

        // Limit the heading level to a maximum of <h6>
        if (level > 6) {
            level = 6;
        }

        // Render the heading tag
        renderer.Write($"<h{level}>");
        renderer.WriteLeafInline(obj);  // Write the content of the heading
        renderer.Write($"</h{level}>");
        renderer.WriteLine();
    }
}
