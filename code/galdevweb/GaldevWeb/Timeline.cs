namespace GaldevWeb
{
    public class Timeline : Dictionary<string, TimelineEntry>
    {
        public Timeline()
        {
        }

        public Timeline(IEnumerable<KeyValuePair<string, TimelineEntry>> collection) : base(collection)
        {
        }

        internal TimelineEntry GetEntry(string name)
        {
            return this[name];
        }

        internal IEnumerable<string> GetNames()
        {
            return Keys;
        }

        internal Timeline GetEntries()
        {
            return this;
        }
    }
}
