namespace GaldevWeb
{
    public class TimelineSeries : Dictionary<string, TimelineEntry>
    {
        public string FirstName => Values.First(x => x.Previous == "").Name;

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

        public TimelineEntry? GetEntryByYear(string year)
        {
            foreach (var entry in Values) {
                if (entry.Year == year) {
                    return entry;
                }
            }
            return null;
        }

        public void ConnectEntries()
        {
            var next = new SortedList<string, TimelineEntry>();

            foreach (var kv in this) {
                next.Add(kv.Value.Year + "-" + kv.Key, kv.Value);
            }

            TimelineEntry? lastEntry = null;
            foreach (var kv in next) {
                var entry = kv.Value;
                if (lastEntry != null) {
                    entry.Previous = lastEntry.Name;
                    lastEntry.Next = entry.Name;
                }
                lastEntry = entry;
            }
        }

        public TimelineEntry? GetNextEntry(string name)
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
