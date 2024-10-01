using Microsoft.AspNetCore.Mvc;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace GaldevWeb.Pages
{
    public class PlainModel : GaldevPageModel
    {
        public TimelineSeries plainTimeline;
        public TimelineEntryList List = new();

        public PlainModel(GaldevApp app) : base(app, "Plain")
        {
            var timeIndex = new TimelineIndex {
                IndexFilePath = app.Config.DataIndexPath,
                Log = app.Log,
            };
            timeIndex.Load(e => true);
            plainTimeline = timeIndex.GetSeriesForLanguage(Lang);
        }

        public ContentResult OnGet(int start = -1, int end = -1, int min = -1)
        {
            var lang = GetLangFromCultureName(UiCultureName);
            //Log.Info($"min={min} start={start} end={end}");
            List = plainTimeline.GetFilteredList(entry => {
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

            var sb = new StringBuilder();
            foreach (var entry in List) {
                var entryString = new StringBuilder();
                entryString.Append(entry.Year);
                entryString.Append('\t');
                entryString.Append(entry.Title);
                entryString.Append('.');

                foreach (var t in entry.Text) {
                    if (Is.Value(t)) {
                        entryString.Append(" # ");
                        entryString.Append(t);
                    }
                }

                //entryString.Append(" | tags=");
                //foreach (var t in entry.Tags) {
                //    if (Is.Value(t)) {
                //        entryString.Append('#');
                //        entryString.Append(t);
                //        entryString.Append(' ');
                //    }
                //}

                //entryString.Append(" | image=");
                //entryString.Append(entry.Image);

                var entryText = entryString.ToString();
                var limitedText = entryText;
                //if (entryText.Length > 32000) {
                //    limitedText = entryText.Substring(0, 32000);
                //    limitedText += "... (truncated)";
                //}
                sb.Append(limitedText);
                sb.Append(" |");
                sb.Append('\n');
            }

            return Content(sb.ToString(), "text/plain; charset=utf-8");
        }
    }
}
