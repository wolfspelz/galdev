using Microsoft.AspNetCore.Mvc;

namespace GaldevWeb.Pages
{
    public class LegacyModel : GaldevPageModel
    {
        public I18nTimelines _timelines;
        public TimelineEntry Entry = new TimelineEntry("(no-name)", "(no-year)", "(no-title)", new string[0]);

        public LegacyModel(GaldevApp app, I18nTimelines timelines) : base(app, "Legacy")
        {
            _timelines = timelines;
        }

        public IActionResult OnGet(string code)
        {
            if (Is.Value(code)) {
                var prefix = code.Substring(0, 1);
                var year = code.Substring(1);

                var lang = prefix switch {
                    "e" => "en",
                    _ => "de",
                };

                var timeline = _timelines.GetEntries(lang);
                var entry = timeline.GetEntryByYear(year);
                var seoTitle = entry.SeoTitle;
                return Redirect($"/Timeline/{seoTitle}");
            }
            return NotFound();
        }

    }
}