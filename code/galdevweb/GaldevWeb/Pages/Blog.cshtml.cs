namespace GaldevWeb.Pages
{
    public class BlogModel : GaldevPageModel
    {
        public BlogIndex Index;
        public BlogPost? Post = null;

        public BlogModel(GaldevApp app) : base(app, "Blog")
        {
            Index = new BlogIndex {
                IndexFilePath = Config.BlogIndexPath
            };
            Index.Load();
        }

        public void OnGet(string name)
        {
            var lang = GetLangFromCultureName(UiCultureName);

            if (Is.Value(name)) {
                name = BlogIndex.GetNameFromSeoTitle(name);
                //Log.Info("", new LogData { [nameof(lang)] = lang, [nameof(name)] = name });
                Post = Index?.GetPost(name, lang);
            }
        }
    }
}