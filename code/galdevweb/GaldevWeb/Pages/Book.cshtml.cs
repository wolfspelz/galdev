namespace GaldevWeb.Pages
{
    public class BookModel : GaldevPageModel
    {
        public bool Export = false;
        public TimelineSeries bookTimeline;
        public TimelineEntryList List = new();

        public BookModel(GaldevApp app) : base(app, "Book")
        {
            var timeIndex = new TimelineIndex {
                IndexFilePath = app.Config.DataIndexPath,
                Log = app.Log,
            };
            timeIndex.Load(entry => !entry.Tags.Contains("_hidden") && !entry.Tags.Contains("_nobook"));
            bookTimeline = timeIndex.GetSeriesForLanguage(Lang);
        }

        public void OnGet(bool export = false, int start = -1, int end = -1, int min = -1)
        {
            var lang = GetLangFromCultureName(UiCultureName);
            //Log.Info($"export={export} min={min} start={start} end={end}");

            Export = export;

            List = bookTimeline.GetFilteredList(entry => {
                var match = true;
                if (match) {
                    if (start > 0) {
                        if (int.TryParse(entry.Year, out var year)) {
                            match = match && year >= start;
                        } else { match = false; }
                    }
                }
                if (match) {
                    if (end > 0) {
                        if (int.TryParse(entry.Year, out var year)) {
                            match = match && year <= end;
                        } else { match = false; }
                    }
                }
                if (match) {
                    if (min > 0) {
                        match = match && entry.TextLen >= min;
                    }
                }
                return match;
            });
        }
    }
}
