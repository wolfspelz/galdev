using GaldevWeb;

namespace GaldevWeb.Pages
{
    public class IndexModel : AppPageModel
    {
        public TimelineEntry? Entry;

        public IndexModel(MyApp app) : base(app, "Index") { }

        public void OnGet(string name)
        {
            if (Is.Value(name)) {
                var lang = GetLangFromCultureName(UiCultureName);
                Log.Info("", new LogData { [nameof(lang)] = lang, [nameof(name)] = name });

                var timeline = new Timeline {
                    IndexFilePath = Config.IndexPath,
                };

                Entry = timeline.GetEntry(name, lang);

            } else {
            }
        }

        protected static string GetLangFromCultureName(string cultureName)
        {
            return cultureName.Split(new char[] { '-', '_' }).First().ToLower();
        }
    }
}