using Microsoft.AspNetCore.Routing;

namespace GaldevWeb.Pages
{
    public class TimelineModel : GaldevPageModel
    {
        public TimelineIndex _timelines;
        public TimelineEntryList List = new();
        public bool NotAvailable = false;
        public TimelineEntry? NextEntry = null;
        public LinkGenerator Links;
        public TimelineSeries Series = new();

        public TimelineModel(GaldevApp app, TimelineIndex timelines, LinkGenerator linkGenerator) : base(app, "Timeline")
        {
            _timelines = timelines;
            Links = linkGenerator;
        }

        public void OnGet(string name)
        {
            var lang = GetLangFromCultureName(UiCultureName);
            var timeline = _timelines.GetSeriesForLanguage(lang);
            Series = timeline;

            if (Is.Value(name)) {
                name = TimelineIndex.GetNameFromSeoTitle(name);
                Log.Info("", new LogData { [nameof(lang)] = lang, [nameof(name)] = name });

                var entry = timeline.GetEntry(name);
                if (entry == null) {
                    NotAvailable = true;

                } else {
                    var totalLen = 0;
                    do {
                        if (entry != null) {
                            totalLen += entry.TextLen;
                            List.Add(entry);
                            entry = timeline.GetNextEntry(entry.Name);
                        }
                    } while (totalLen < Config.EntryPageTextLength && entry != null);

                    NextEntry = entry;
                }
            }
        }
    }
}