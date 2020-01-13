using System.Collections;
using System.Collections.Generic;

namespace galdevtool
{
    public class TimelineEntry : object
    {
        public string Name { get; set; } = "";
        public string Year { get; set; } = "";
        public string Title { get; set; } = "";
        public string Short { get; set; } = "";
        public string Summary { get; set; } = "";
        public string Headline { get; set; } = "";
        public string Image { get; set; } = "";
        public string Smallimage { get; set; } = "";
        public string Twitter { get; set; } = "";
        public string Twitterimage { get; set; } = "";
        public string Facebook { get; set; } = "";
        public string Facebook2 { get; set; } = "";
        public string Facebook3 { get; set; } = "";
        public string Facebookimage { get; set; } = "";
        public string Post { get; set; } = "";
        public string Postimage { get; set; } = "";
        public string Author { get; set; } = "";
        public string Translation { get; set; } = "";
        public List<string> Topics { get; set; } = new List<string>();
        public List<string> Tags { get; set; } = new List<string>();
        public List<string> Text { get; set; } = new List<string>();

        public new bool Equals(object oy)
        {
            var x = this;
            var y = oy as TimelineEntry;
            if (y == null) return false;
            if (x.Name != y.Name) return false;
            if (x.Year != y.Year) return false;
            if (x.Title != y.Title) return false;
            if (x.Short != y.Short) return false;
            if (x.Summary != y.Summary) return false;
            if (x.Headline != y.Headline) return false;
            if (x.Image != y.Image) return false;
            if (x.Smallimage != y.Smallimage) return false;
            if (x.Twitter != y.Twitter) return false;
            if (x.Twitterimage != y.Twitterimage) return false;
            if (x.Facebook != y.Facebook) return false;
            if (x.Facebook2 != y.Facebook2) return false;
            if (x.Facebook3 != y.Facebook3) return false;
            if (x.Facebookimage != y.Facebookimage) return false;
            if (x.Post != y.Post) return false;
            if (x.Postimage != y.Postimage) return false;
            if (x.Author != y.Author) return false;
            if (x.Translation != y.Translation) return false;
            if (string.Join("", x.Tags) != string.Join("", y.Tags)) return false;
            if (string.Join("", x.Topics) != string.Join("", y.Topics)) return false;
            if (string.Join("", x.Text) != string.Join("", y.Text)) return false;
            return true;
        }

        public int GetHashCode(object obj)
        {
            throw new System.NotImplementedException();
        }
    }
}