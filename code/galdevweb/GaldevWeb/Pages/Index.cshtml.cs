using GaldevWeb;

namespace GaldevWeb.Pages
{
    public class IndexModel : AppPageModel
    {
        public IEnumerable<string>? Names;
        public TimelineEntry? Entry;

        public IndexModel(MyApp app) : base(app, "Index") { }

        public void OnGet(string name)
        {
            var lang = GetLangFromCultureName(UiCultureName);

            var timeline = new Timeline {
                IndexFilePath = Config.IndexPath,
            };

            if (Is.Value(name)) {
                Log.Info("Entry", new LogData { [nameof(lang)] = lang, [nameof(name)] = name });
                Entry = timeline.GetEntry(name, lang);
            } else {
                Log.Info("Names", new LogData { [nameof(lang)] = lang });
                Names = timeline.GetNames(lang);
            }
        }

        protected static string GetLangFromCultureName(string cultureName)
        {
            return cultureName.Split(new char[] { '-', '_' }).First().ToLower();
        }
    }
}