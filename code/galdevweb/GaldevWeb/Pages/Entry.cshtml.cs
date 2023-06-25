namespace GaldevWeb.Pages
{
    public class EntryModel : GaldevPageModel
    {
        public I18nTimelines _timelines;
        public TimelineEntry Entry = new TimelineEntry("(no-name)", "(no-year)", "(no-title)", new string[0]);

        public EntryModel(GaldevApp app, I18nTimelines timelines) : base(app, "Entry")
        {
            _timelines = timelines;
        }

        public void OnGet(string name)
        {
            var lang = GetLangFromCultureName(UiCultureName);
            var timeline = _timelines.GetEntries(lang);

            if (Is.Value(name)) {
                name = I18nTimelines.GetNameFromSeoTitle(name);
                Log.Info("Entry", new LogData { [nameof(lang)] = lang, [nameof(name)] = name });
                Entry = timeline.GetEntry(name);
            }
        }
    }
}