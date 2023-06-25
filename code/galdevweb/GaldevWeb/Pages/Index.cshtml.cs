namespace GaldevWeb.Pages
{
    public class IndexModel : GaldevPageModel
    {
        public I18nTimelines _timelines;
        public Timeline Entries = new Timeline();

        public IndexModel(GaldevApp app, I18nTimelines timelines) : base(app, "Index")
        {
            _timelines = timelines;
        }

        public void OnGet()
        {
            var lang = GetLangFromCultureName(UiCultureName);
            var timeline = _timelines.GetEntries(lang, entry => entry.TextLen > Config.ListMinTextLength);

            Log.Info("Index", new LogData { [nameof(lang)] = lang });
            Entries = timeline.GetEntries();
        }
    }
}