namespace GaldevWeb.Pages
{
    public class ListModel : GaldevPageModel
    {
        public TimelineEntryList List = new();

        public ListModel(GaldevApp app) : base(app, "List")
        {
        }

        public void OnGet(int length = -1, string date = "")
        {
            length = length < 0 ? Config.ListMinTextLength : length;

            if (!DateTime.TryParse(date, out DateTime minDate)) {
                minDate = DateTime.MinValue;
            }

            var lang = GetLangFromCultureName(UiCultureName);

            List = Timeline.GetFilteredList(entry => entry.TextLen >= length && entry.ChangedDate >= minDate);
        }
    }
}