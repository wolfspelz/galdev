namespace GaldevWeb
{
    public class TimelineEntry
    {
        public string Name;
        public string Year;
        public string Title;
        public string[] Text;
        public string? Summary;
        public string? Image;

        public TimelineEntry(string name, string year, string title, string[] text)
        {
            Name = name;
            Year = year;
            Title = title;
            Text = text;
        }

        public string SeoTitle => $"{Name}-{Year}-{Title}".Replace("/", "-");
    }
}
