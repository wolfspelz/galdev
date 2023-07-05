using Microsoft.CodeAnalysis.Diagnostics;
using System.Linq;

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
                    var part0 = Headline.Split(":", 2)[0];
                    if (!part0.Contains("HEADLINE")) {
                        if (part0.Length < Title.Length) {
                            s = part0;
                        }
                    }
                }
                if (string.IsNullOrEmpty(s)) { s = Title; }
                return s;
            }
        }

    }
}
