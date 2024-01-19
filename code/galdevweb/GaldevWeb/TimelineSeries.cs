using static System.Net.Mime.MediaTypeNames;

namespace GaldevWeb
{
    public class TimelineSeries : Dictionary<string, TimelineEntry>
    {
        public string FirstName => Values.First(x => x.Previous == "").Name;

        private readonly Dictionary<string, TimelineEntryList> _topicsById = new();
        private readonly Dictionary<string, string> _topicDisplayNameById = new();
        private readonly Dictionary<string, string> _nameByAlias = new();

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

        public TimelineEntry? GetEntry(string nameOrAlias)
        {
            {
                var nameToLower = nameOrAlias.ToLower();
                if (HasEntry(nameToLower)) {
                    return this[nameToLower];
                }
            }
            if (_nameByAlias.ContainsKey(nameOrAlias)) {
                var nameToLower = _nameByAlias[nameOrAlias].ToLower();
                if (HasEntry(nameToLower)) {
                    return this[nameToLower];
                }
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

        public string[] Topics => _topicsById.Keys.ToArray();

        public string GetTitleOfTopic(string topic)
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

        public void CreateAliases()
        {
            foreach (var kv in this) {
                var name = kv.Key;
                var entry = kv.Value;
                _nameByAlias[entry.Name] = name;
                foreach (var alias in entry.Aliases) {
                    _nameByAlias[alias.ToLower()] = name;
                }
            }
        }

    }
}
