﻿namespace GaldevWeb
{
    public class I18nTimeline
    {
        public string IndexFilePath { get; set; } = "(IndexFilePath)";
        public int MinEntryTextLength { get; set; } = 1000;
        public string[] Languages { get; set; } = new[] { "en", "de", };
        public IDataProvider DataProvider { get; set; } = new FileDataProvider();

        private Dictionary<string, Timeline> _entries = new Dictionary<string, Timeline>();
        public Timeline GetEntries(string lang) => _entries[lang];

        internal void Init()
        {
            foreach (var lang in Languages) {
                var names = GetNames(lang);
                var entries = new Timeline();
                foreach (var name in names) {
                    var entry = GetEntry(name, lang);
                    entries.Add(name, entry);
                }
                _entries.Add(lang, entries);
            }
        }

        public IEnumerable<string> GetNames(string lang)
        {
            var indexData = DataProvider.GetData(IndexFilePath);
            var indexNode = JsonPath.Node.FromYaml(indexData, new YamlDeserializer.Options { LowerCaseDictKeys = true });
            var names = indexNode["entries"]
                .AsDictionary
                .Where(kv => Is.Value(kv.Value[lang]))
                .Select(kv => kv.Key)
                ;
            ;
            var result = new List<string>();
            foreach (var name in names) {
                var entry = GetEntry(name, lang);
                var textLength = entry.Text.Aggregate(0, (acc, x) => acc + x.Length);
                if (entry != null && textLength > MinEntryTextLength) {
                    result.Add(name);
                }
            }
            return result;
        }

        public TimelineEntry GetEntry(string name, string lang)
        {
            var id = I18nTimeline.GetNameFromSeoTitle(name);

            var indexData = DataProvider.GetData(IndexFilePath);
            var indexNode = JsonPath.Node.FromYaml(indexData, new YamlDeserializer.Options { LowerCaseDictKeys = true });

            var langPath = indexNode["languages"][lang]["path"].AsString;
            var entryFileName = indexNode["entries"][id][lang].AsString;
            if (Is.Empty(entryFileName)) {
                throw new Exception($"No entry={id} for lang={lang} in index={IndexFilePath}");
            }
            if (entryFileName.EndsWith(".yaml")) {
                entryFileName = entryFileName.Substring(0, entryFileName.Length - ".yaml".Length);
            }
            if (entryFileName.EndsWith(".yml")) {
                entryFileName = entryFileName.Substring(0, entryFileName.Length - ".yml".Length);
            }

            var dirPath = Path.GetDirectoryName(IndexFilePath);
            var entryPath = Path.Combine(dirPath ?? "", langPath, entryFileName);
            var entryPathWithExt = entryPath + ".yml";
            if (!DataProvider.HasData(entryPathWithExt)) {
                entryPathWithExt = entryPath + ".yaml";
                if (!DataProvider.HasData(entryPathWithExt)) {
                    throw new Exception($"No entry file= {entryPathWithExt}");
                }
            }
            var entryData = DataProvider.GetData(entryPathWithExt);
            var entryNode = JsonPath.Node.FromYaml(entryData, new YamlDeserializer.Options { LowerCaseDictKeys = true });

            var year = entryNode["year"].AsString;
            var title = entryNode["title"].AsString;
            var text = entryNode["text"].AsString
                .Replace("\r\n", "\n")
                .Replace("\r", "\n")
                .Split('\n')
                .Select(x => x.Trim())
                .ToArray();

            var entry = new TimelineEntry(id, year, title, text);

            var summary = entryNode["summary"].AsString;
            if (Is.Value(summary)) {
                entry.Summary = summary;
            }

            var shortSummary = entryNode["short"].AsString;
            if (Is.Value(shortSummary)) {
                entry.Summary = shortSummary;
            }

            var image = entryNode["image"].AsString;
            if (Is.Value(image)) {
                entry.Image = $"{lang}/{image}";
            }

            return entry;
        }

        protected static string GetNameFromSeoTitle(string seoTitle)
        {
            var parts = seoTitle.ToLower().Split(new char[] { '-', ':' }, 2);
            return parts[0];
        }

        public static string GetSeoTitle(TimelineEntry entry)
        {
            return $"{entry.Name}-{entry.Year}-{entry.Title}";
        }

        internal string GetImagePath(string imageName, string lang)
        {
            var indexData = DataProvider.GetData(IndexFilePath);
            var indexNode = JsonPath.Node.FromYaml(indexData, new YamlDeserializer.Options { LowerCaseDictKeys = true });
            var langPath = indexNode["languages"][lang]["images"].AsString;
            return Path.Combine(Path.GetDirectoryName(IndexFilePath) ?? "", langPath, imageName);
        }

    }
}
