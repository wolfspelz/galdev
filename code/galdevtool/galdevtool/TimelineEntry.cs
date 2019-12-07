using System.Collections.Generic;

namespace galdevtool
{
    public class TimelineEntry
    {
        public string Year { get; set; } = "";
        public string Title { get; set; } = "";
        public string Short { get; set; } = "";
        public string Summary { get; set; } = "";
        public string Headline { get; set; } = "";
        public string Image { get; set; } = "";
        public string Smallimage { get; set; } = "";
        public List<string> Tags { get; set; } = new List<string>();
        public string Twitter { get; set; } = "";
        public string Twitterimage { get; set; } = "";
        public string Post { get; set; } = "";
        public string Postimage { get; set; } = "";
        public List<string> Topics { get; set; } = new List<string>();
        public List<string> Text { get; set; } = new List<string>();
    }
}