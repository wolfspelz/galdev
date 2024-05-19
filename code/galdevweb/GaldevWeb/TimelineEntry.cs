using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Linq;

namespace GaldevWeb
{
    public class TimelineEntry
    {
        public string Name;
        public string Year;
        public string Title;
        public string Filename;
        public string[] Text;

        public string? ShortTitle;
        public string? Summary;
        public string? Image;
        public string[] Topics = new string[0];
        public string[] Aliases = new string[0];
        public string[] Tags = new string[0];

        public string Next = "";
        public string Previous = "";
        public string Headline = "";
        
        public Dictionary<string, string> _sequenceText = new();
        public Dictionary<string, string> _sequenceNext = new();

        public string SequenceText(string name)
        {
            if (_sequenceText.ContainsKey(name)) {
                return _sequenceText[name];
            }
            return "";
        }

        public override string ToString() => $"{Year} {Name} {Title}";

        public TimelineEntry(string name, string year, string title, string filename, string[] text)
        {
            Name = name;
            Year = year;
            Title = title;
            Filename = filename;
            Text = text;
        }

        public string SeoTitle => $"{Name}-{Year}-{Title}"
            .Replace("/", "-")
            .Replace(" ", "_")
            .Replace(":", "_")
            .Replace("\"", "")
            ;

        public int TextLen => Text.Aggregate(0, (acc, x) => acc + x.Length);

        public string Description
        {
            get {
                var s = Title;
                if (string.IsNullOrEmpty(s)) {
                    s = Headline;
                }
                if (string.IsNullOrEmpty(s)) {
                    s = ShortTitle;
                }
                if (string.IsNullOrEmpty(s)) {
                    s = Summary;
                }
                if (string.IsNullOrEmpty(s)) {
                    s = "";
                }
                return s;
            }
        }

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

        public string FullTextForSearch
        {
            get {
                var s = "";
                var sep = "|";
                s += sep + Name;
                s += sep + Year ;
                s += sep + Title;
                s += ShortTitle != null ? sep + ShortTitle : "";
                s += Summary != null ? sep + Summary : "";
                s += Headline != null ? sep + Headline : "";
                foreach (var t in Text) {
                    s += sep + t;
                }
                foreach (var t in Topics) {
                    s += sep + t;
                }
                foreach (var t in Tags) {
                    s += sep + t;
                }
                return s;
            }
        }
    }
}
