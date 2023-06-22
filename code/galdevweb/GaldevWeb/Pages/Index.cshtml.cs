using GaldevWeb;

namespace GaldevWeb.Pages
{
    public class IndexModel : AppPageModel
    {
        public I18nTimeline _timelines;
        public Timeline? Entries;
        public TimelineEntry? Entry;

        public IndexModel(MyApp app, I18nTimeline timelines) : base(app, "Index")
        {
            _timelines = timelines;
        }

        public void OnGet(string name)
        {
            var lang = GetLangFromCultureName(UiCultureName);

            //var timeline = new Timeline {
            //    IndexFilePath = Config.IndexPath,
            //};

            var timeline = _timelines.GetEntries(lang);

            if (Is.Value(name)) {
                Log.Info("Entry", new LogData { [nameof(lang)] = lang, [nameof(name)] = name });
                Entry = timeline.GetEntry(name);
            } else {
                Log.Info("Entries", new LogData { [nameof(lang)] = lang });
                Entries = timeline.GetEntries();
            }
        }

        protected static string GetLangFromCultureName(string cultureName)
        {
            return cultureName.Split(new char[] { '-', '_' }).First().ToLower();
        }
    }
}