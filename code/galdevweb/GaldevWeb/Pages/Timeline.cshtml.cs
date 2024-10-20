using Markdig.Extensions.DefinitionLists;

namespace GaldevWeb.Pages
{
    public class TimelineModel : GaldevPageModel
    {
        public TimelineEntryList List = new();
        public bool NotAvailable = false;
        public TimelineEntry? NextEntry = null;
        public TimelineEntry? PreviousEntry = null;
        public string Term = "";

        public TimelineModel(GaldevApp app) : base(app, "Timeline")
        {
        }

        public void OnGet(string name, string term = "")
        {
            if (Is.Value(name)) {
                Term = term;

                name = TimelineIndex.GetNameFromSeoTitle(name);
                //Log.Info("", new LogData { [nameof(name)] = name });

                var entry = Timeline.GetEntry(name);
                if (entry == null) {
                    NotAvailable = true;

                } else {
                    if (Is.Value(entry.Previous)) {
                        PreviousEntry = Timeline.GetEntry(entry.Previous);
                    }
                    
                    var totalLen = 0;
                    do {
                        if (entry != null) {
                            totalLen += entry.TextLen;
                            List.Add(entry);
                            entry = Timeline.GetNextEntry(entry.Name);
                        }
                    } while (totalLen < Config.EntryPageTextLength && entry != null);

                    NextEntry = entry;
                }
            }
        }
    }
}