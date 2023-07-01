namespace GaldevWeb
{
    public class TimelineSeries : Dictionary<string, TimelineEntry>
    {
        public string FirstName => Values.First(x => x.Previous == "").Name;

        private readonly Dictionary<string, TimelineEntryList> _topicsById = new();
        private readonly Dictionary<string, string> _topicDisplayNameById = new();

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

        public TimelineEntryList GetFilteredList(TimelineEntryCondition? filter = null)
        {
            var result = new TimelineEntryList();

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

        public void CreateTopics(Dictionary<string, string> topics)
        {
            //_topicsById
            foreach (var kv in topics) {
                var topic = kv.Key;
                var entriesWithTopic = GetFilteredList(entry => entry.Topics.Contains(topic));
                _topicsById[topic] = entriesWithTopic;
                _topicDisplayNameById[topic] = kv.Value;
            }
        }

        public string GetTopicDisplayName(string topic)
        {
            topic = topic.ToLower();
            if (_topicDisplayNameById.ContainsKey(topic)) {
                return _topicDisplayNameById[topic];
            }
            return "";
        }

        public TimelineEntryList GetEntriesOfTopic(string topic)
        {
            topic = topic.ToLower();
            if (_topicsById.ContainsKey(topic)) {
                return _topicsById[topic];
            }
            return new TimelineEntryList();
        }

    }
}
