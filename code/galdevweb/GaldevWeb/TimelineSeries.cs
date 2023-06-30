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

        public delegate bool TimelineEntryCondition(TimelineEntry entry);

        public List<TimelineEntry> GetFilteredList(TimelineEntryCondition? filter = null)
        {
            var result = new List<TimelineEntry>();

            var name = FirstName;
            while (Is.Value(name)) {
                var entry = GetEntry(name);
                if (entry != null) {
                    if (filter == null || filter(entry)) {
                        result.Add(entry);
                    }
                    name = entry.Next;
                } else {
                    name = "";
                }
            }

            return result;
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
            var entry = GetEntry(name);
            if (entry != null) {
                return GetEntry(entry.Next);
            }
            return null;
        }
    }
}
