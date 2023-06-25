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

        public bool HasEntry(string name)
        {
            name = name.ToLower();
            return this.ContainsKey(name);
        }

        public TimelineEntry GetEntry(string name)
        {
            name = name.ToLower();
            if (this.ContainsKey(name)) {
                return this[name];
            }
            throw new KeyNotFoundException($"Timeline entry '{name}' not found");
        }

        public IEnumerable<string> GetNames()
        {
            return Keys;
        }

        public Timeline GetEntries()
        {
            return this;
        }
    }
}
