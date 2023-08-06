using Microsoft.AspNetCore.Routing;

namespace GaldevWeb.Pages
{
    public class TimelineModel : GaldevPageModel
    {
        public TimelineEntryList List = new();
        public bool NotAvailable = false;
        public TimelineEntry? NextEntry = null;

        public TimelineModel(GaldevApp app) : base(app, "Timeline")
        {
        }

        public void OnGet(string name)
        {
            if (Is.Value(name)) {
                name = TimelineIndex.GetNameFromSeoTitle(name);
                Log.Info("", new LogData { [nameof(name)] = name });

                var entry = base.Timeline.GetEntry(name);
                if (entry == null) {
                    NotAvailable = true;

                } else {
                    var totalLen = 0;
                    do {
                        if (entry != null) {
                            totalLen += entry.TextLen;
                            List.Add(entry);
                            entry = base.Timeline.GetNextEntry(entry.Name);
                        }
                    } while (totalLen < Config.EntryPageTextLength && entry != null);

                    NextEntry = entry;
                }
            }
        }
    }
}