namespace GaldevWeb.Pages
{
    public class FindModel : GaldevPageModel
    {
        public TimelineIndex _timelines;
        public TimelineEntryList List = new();

        public FindModel(GaldevApp app, TimelineIndex timelines) : base(app, "List")
        {
            _timelines = timelines;
        }

        public void OnGet(string term)
        {
            var lang = GetLangFromCultureName(UiCultureName);
            Log.Info("", new LogData { [nameof(lang)] = lang });
            var timeline = _timelines.GetSeriesForLanguage(lang);

            foreach (var kv in timeline) {
                var entry = kv.Value;
                if (entry.FullTextForSearch.ToLower().Contains(term)) {
                    List.Add(entry);
                }
            }
        }
    }
}