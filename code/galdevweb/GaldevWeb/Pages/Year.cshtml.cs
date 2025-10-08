using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GaldevWeb.Pages;

public class YearModel : GaldevPageModel
{
    public TimelineEntry Entry = new TimelineEntry("(no-name)", "(no-year)", "(no-title)", "(no-filename)", new string[0]);
    public bool NotAvailable = false;

    public YearModel(GaldevApp app) : base(app, "Year")
    {
    }

    public IActionResult OnGet(string year)
    {
        if (Is.Value(year)) {
            var lang = GetLangFromCultureName(UiCultureName);
            //Log.Info("", new LogData { [nameof(year)] = year });
            var entry = Timeline.GetEntryByYear(year);
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