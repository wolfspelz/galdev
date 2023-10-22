namespace GaldevWeb.Pages
{
    public class BookExportModel : GaldevPageModel
    {
        public TimelineEntryList List = new();

        public BookExportModel(GaldevApp app) : base(app, "BookExport")
        {
        }

        public void OnGet()
        {
            var lang = GetLangFromCultureName(UiCultureName);
            Log.Info("");
            List = Timeline.GetFilteredList(entry => !entry.Tags.Contains("_nobook"));
        }
    }
}
