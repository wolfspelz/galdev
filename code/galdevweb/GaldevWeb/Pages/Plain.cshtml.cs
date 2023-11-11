namespace GaldevWeb.Pages
{
    public class PlainModel : GaldevPageModel
    {
        public TimelineEntryList List = new();

        public PlainModel(GaldevApp app) : base(app, "Plain")
        {
        }

        public void OnGet(int start = -1, int end = -1, int min = -1)
        {
            var lang = GetLangFromCultureName(UiCultureName);
            Log.Info($"min={min} start={start} end={end}");
            List = Timeline.GetFilteredList(entry => {
                var match = true;
                match = match && !entry.Tags.Contains("_nobook");
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
