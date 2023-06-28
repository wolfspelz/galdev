using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.RegularExpressions;

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

                var timeline = _timelines.GetEntries(lang);
                var entry = timeline.GetEntryByYear(year);
                var seoTitle = entry?.SeoTitle;
                var urlEscapedSeoTitle = WebUtility.UrlEncode(seoTitle);
                var culture = new Dictionary<string, string> {
                    { "de", "de-DE" },
                    { "en", "en-US" },
                }[lang];

                Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                    );

                return Redirect($"/Timeline/{urlEscapedSeoTitle}");
            }
            return NotFound();
        }

    }
}