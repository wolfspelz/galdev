using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GaldevWeb.Pages
{
    public class YearModel : GaldevPageModel
    {
        public TimelineIndex _timelines;
        public TimelineEntry Entry = new TimelineEntry("(no-name)", "(no-year)", "(no-title)", new string[0]);
        public bool NotAvailable = false;

        public YearModel(GaldevApp app, TimelineIndex timelines) : base(app, "Year")
        {
            _timelines = timelines;
        }

        public IActionResult OnGet(string year)
        {
            if (Is.Value(year)) {
                var lang = GetLangFromCultureName(UiCultureName);
                Log.Info("", new LogData { [nameof(lang)] = lang, [nameof(year)] = year });
                var timeline = _timelines.GetSeriesForLanguage(lang);
                var entry = timeline.GetEntryByYear(year);
                if (entry == null) {
                    NotAvailable = true;
                    return Page();

                } else {
                    var seoTitle = entry?.SeoTitle;
                    var urlEscapedSeoTitle = WebUtility.UrlEncode(seoTitle);
                    return Redirect($"/Timeline/{urlEscapedSeoTitle}");
                }
            }
            return NotFound();
        }

    }
}