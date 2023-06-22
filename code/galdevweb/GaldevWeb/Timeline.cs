namespace GaldevWeb
{
    public class Timeline : Dictionary<string, TimelineEntry>
    {
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
