using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GaldevWeb.Pages
{
    public class LegacyModel : GaldevPageModel
    {
        public TimelineEntry Entry = new TimelineEntry("(no-name)", "(no-year)", "(no-title)", "(no-filename)", new string[0]);

        public LegacyModel(GaldevApp app) : base(app, "Legacy")
        {
        }

        public IActionResult OnGet(string code)
        {
            if (Is.Value(code)) {
                //Log.Info("", new LogData { [nameof(code)] = code });
                var prefix = code.Substring(0, 1);
                var year = code.Substring(1);

                var lang = prefix switch {
                    "e" => "en",
                    _ => "de",
                };

                var entry = Timeline.GetEntryByYear(year);
                var seoTitle = entry?.SeoTitle;
                var urlEscapedSeoTitle = WebUtility.UrlEncode(seoTitle);
                return Redirect($"/Timeline/{urlEscapedSeoTitle}");
            }
            return NotFound();
        }

    }
}