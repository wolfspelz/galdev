using Microsoft.CodeAnalysis.Diagnostics;

namespace GaldevWeb
{
    public class TimelineEntry
    {
        public string Name;
        public string Year;
        public string Title;
        public string[] Text;

        public string? ShortTitle;
        public string? Summary;
        public string? Image;
        public string[] Topics = new string[0];

        public string Next = "";
        public string Previous = "";
        public string Headline = "";

        public TimelineEntry(string name, string year, string title, string[] text)
        {
            Name = name;
            Year = year;
            Title = title;
            Text = text;
        }

        public string SeoTitle => $"{Name}-{Year}-{Title}".Replace("/", "-").Replace(" ", "_");
        public int TextLen => Text.Aggregate(0, (acc, x) => acc + x.Length);
        public string DisplayName
        {
            get {
                var s = ShortTitle;
                if (string.IsNullOrEmpty(s)) {
                    var parts = Headline.Split(":", 2);
                    s = parts[0];
                }
                if (string.IsNullOrEmpty(s)) { s = Title; }
                return s;
            }
        }

    }
}
