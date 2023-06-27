namespace GaldevWeb
{
    public class TimelineSeries : Dictionary<string, TimelineEntry>
    {
        public TimelineSeries()
        {
        }

        public TimelineSeries(IEnumerable<KeyValuePair<string, TimelineEntry>> collection) : base(collection)
        {
        }

        public IEnumerable<string> GetAllNames()
        {
            return Keys;
        }

        public TimelineSeries GetAllEntries()
        {
            return this;
        }

        public bool HasEntry(string name)
        {
            name = name.ToLower();
            return this.ContainsKey(name);
        }

        public TimelineEntry? GetEntry(string name)
        {
            name = name.ToLower();
            if (HasEntry(name)) {
                return this[name];
            }
            return null;
        }

        internal TimelineEntry? GetEntryByYear(string year)
        {
            foreach (var entry in Values) {
                if (entry.Year == year) {
                    return entry;
                }
            }
            return null;
        }

        internal TimelineEntry? GetNextEntry(string name)
        {
            var takeNext = false;
            foreach (var kv in this) {
                if (takeNext) {
                    return kv.Value;
                }
                if (kv.Key == name) {
                    takeNext = true;
                }
            }
            return null;
        }
    }
}
