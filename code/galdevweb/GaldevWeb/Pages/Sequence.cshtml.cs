namespace GaldevWeb.Pages
{
    public class SequenceModel : GaldevPageModel
    {
        public string Name = "";
        public string Title = "";
        public string Summary = "";
        public TimelineEntryList List = new();

        public SequenceModel(GaldevApp app) : base(app, "Sequence")
        {
        }

        public void OnGet(string name)
        {
            Log.Info("", new LogData { [nameof(name)] = name });

            var sequence = Timeline.GetSequence(name);
            if (sequence != null) {
                Name = name;
                Title = sequence.Title;
                Summary = sequence.Summary;

                foreach (var entryName in sequence.Entries) {
                    var entry = Timeline.GetEntry(entryName);
                    if (entry != null) {
                        List.Add(entry);
                    }
                }
            }
        }
    }
}