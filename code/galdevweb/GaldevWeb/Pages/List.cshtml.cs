namespace GaldevWeb.Pages
{
    public class ListModel : GaldevPageModel
    {
        public TimelineEntryList List = new();

        public ListModel(GaldevApp app) : base(app, "List")
        {
        }

        public void OnGet(int min = -1)
        {
            min = min < 0 ? Config.ListMinTextLength : min;
            var lang = GetLangFromCultureName(UiCultureName);
            //Log.Info("", new LogData { [nameof(min)] = min });
            List = Timeline.GetFilteredList(entry => entry.TextLen > min);
        }
    }
}