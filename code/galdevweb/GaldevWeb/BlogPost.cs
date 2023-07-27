namespace GaldevWeb
{
    public class BlogPost
    {
        public string Name = "";
        public string Language = "";
        public string Title = "";
        public string Time = "";
        public string Author = "";
        public string Summary = "";
        public string[] Tags = Array.Empty<string>();
        public string Image = "";
        public string Html = "";
        public string Text = "";

        public string SeoTitle => $"{Name}-{Title}".Replace("/", "-").Replace(" ", "_");
    }
}
