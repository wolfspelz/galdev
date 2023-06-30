namespace GaldevWeb.Pages
{
    public class TimelineModel : GaldevPageModel
    {
        public TimelineIndex _timelines;
        public List<TimelineEntry> Entries = new();
        public bool NotAvailable = false;
        public TimelineEntry? NextEntry = null;

        public TimelineModel(GaldevApp app, TimelineIndex timelines) : base(app, "Timeline")
        {
            _timelines = timelines;
        }

        public void OnGet(string name)
        {
            var lang = GetLangFromCultureName(UiCultureName);
            var timeline = _timelines.GetSeriesForLanguage(lang);

            if (Is.Value(name)) {
                name = TimelineIndex.GetNameFromSeoTitle(name);
                Log.Info("Timeline", new LogData { [nameof(lang)] = lang, [nameof(name)] = name });

                var entry = timeline.GetEntry(name);
                if (entry == null) {
                    NotAvailable = true;

                } else {
                    var totalLen = 0;
                    do {
                        if (entry != null) {
                            totalLen += entry.TextLen;
                            Entries.Add(entry);
                            entry = timeline.GetNextEntry(entry.Name);
                        }
                    } while (totalLen < Config.EntryPageTextLength && entry != null);

                    NextEntry = entry;
                }
            }
        }
    }
}