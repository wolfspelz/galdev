using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace GaldevWeb;

public class CustomHeadingRendererExtension : IMarkdownExtension
{
    public string Prefix { get; set; } = "";
    public string Suffix { get; set; } = "";

    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        // No changes needed during the pipeline building process
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        // Customize the HTML renderer for headings
        if (renderer is HtmlRenderer htmlRenderer) {
            // Remove the existing heading renderer if present
            var headingRenderer = htmlRenderer.ObjectRenderers.FindExact<HeadingRenderer>();
            if (headingRenderer != null) {
                htmlRenderer.ObjectRenderers.Remove(headingRenderer);
            }

            // Add the custom heading renderer
            htmlRenderer.ObjectRenderers.Add(new CustomHeadingRenderer() { Prefix = Prefix, Suffix = Suffix });
        }
    }
}

public class CustomHeadingRenderer : HtmlObjectRenderer<HeadingBlock>
{
    public string Prefix { get; set; } = "";
    public string Suffix { get; set; } = "";

    protected override void Write(HtmlRenderer renderer, HeadingBlock obj)
    {
        // Modify the heading content by adding the prefix and suffix
        renderer.Write(Prefix);
        renderer.WriteLeafInline(obj);
        renderer.Write(Suffix);
        renderer.WriteLine();
    }
}
