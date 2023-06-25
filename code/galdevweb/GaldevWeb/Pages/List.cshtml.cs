namespace GaldevWeb.Pages
{
    public class ListModel : GaldevPageModel
    {
        public I18nTimelines _timelines;
        public Timeline Entries = new Timeline();

        public ListModel(GaldevApp app, I18nTimelines timelines) : base(app, "List")
        {
            _timelines = timelines;
        }

        public void OnGet(string name)
        {
            var lang = GetLangFromCultureName(UiCultureName);
            var timeline = _timelines.GetEntries(lang, entry => entry.TextLen > Config.ListMinTextLength);

            Log.Info("List", new LogData { [nameof(lang)] = lang });
            Entries = timeline.GetEntries();
        }
    }
}