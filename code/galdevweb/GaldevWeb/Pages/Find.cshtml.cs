namespace GaldevWeb.Pages
{
    public class FindModel : GaldevPageModel
    {
        public TimelineEntryList List = new();
        public string SearchTerm = "";

        public FindModel(GaldevApp app) : base(app, "List")
        {
        }

        public void OnGet(string term)
        {
            Log.Info("", new LogData { [nameof(term)] = term });
            SearchTerm = term.ToLower();

            foreach (var kv in Timeline) {
                var entry = kv.Value;
                if (entry.FullTextForSearch.ToLower().Contains(SearchTerm)) {
                    List.Add(entry);
                }
            }

            ViewData["SearchTerm"] = SearchTerm;
        }
    }
}