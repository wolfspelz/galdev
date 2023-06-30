using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GaldevWeb.Pages
{
    public class LegacyModel : GaldevPageModel
    {
        public TimelineIndex _timelines;
        public TimelineEntry Entry = new TimelineEntry("(no-name)", "(no-year)", "(no-title)", new string[0]);

        public LegacyModel(GaldevApp app, TimelineIndex timelines) : base(app, "Legacy")
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

                var timeline = _timelines.GetSeriesForLanguage(lang);
                var entry = timeline.GetEntryByYear(year);
                var seoTitle = entry?.SeoTitle;
                var urlEscapedSeoTitle = WebUtility.UrlEncode(seoTitle);
                return Redirect($"/Timeline/{urlEscapedSeoTitle}");
            }
            return NotFound();
        }

    }
}