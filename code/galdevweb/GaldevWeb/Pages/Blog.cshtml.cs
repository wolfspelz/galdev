namespace GaldevWeb.Pages
{
    public class BlogModel : GaldevPageModel
    {
        public BlogIndex Index = new BlogIndex();
        public BlogPost? Post = null;

        public LinkGenerator Links;

        public BlogModel(GaldevApp app, LinkGenerator linkGenerator) : base(app, "Blog")
        {
            Index.Load();

            Links = linkGenerator;
        }

        public void OnGet(string name)
        {
            var lang = GetLangFromCultureName(UiCultureName);

            if (Is.Value(name)) {
                name = BlogIndex.GetNameFromSeoTitle(name);
                Log.Info("", new LogData { [nameof(lang)] = lang, [nameof(name)] = name });
                Post = Index?.GetPost(name, lang);
            }
        }
    }
}