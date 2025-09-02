namespace GaldevWeb;

public class TimelineLanguage
{
    public string Id;
    public string TextPath;
    public string ImagePath;
    public Dictionary<string, string> Topics;

    public TimelineLanguage(string lang, string path, string imagePath, Dictionary<string, string> topics)
    {
        Id = lang;
        TextPath = path;
        ImagePath = imagePath;
        Topics = topics;
    }
}
