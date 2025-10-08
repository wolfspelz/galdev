namespace GaldevWeb;

public class TimelineSequence
{
    public string Title { get; }
    public string Summary { get; }
    public IEnumerable<string> Entries { get; }

    public TimelineSequence(string title, string summary, IEnumerable<string> entries)
    {
        Title = title;
        Summary = summary;
        Entries = entries;
    }
}
