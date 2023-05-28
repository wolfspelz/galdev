namespace GaldevWeb.Pages
{
    public class IndexModel : AppPageModel
    {
        public string Test = "A";

        public IndexModel(MyApp app) : base(app, "Index") { }

        public void OnGet()
        {
            Test = RandomString.Alphanum(10);
        }
    }
}