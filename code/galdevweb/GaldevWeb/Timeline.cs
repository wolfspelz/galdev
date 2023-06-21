namespace GaldevWeb
{
    public class Timeline
    {
        public string IndexFilePath { get; set; } = "(IndexFilePath)";
        public IDataProvider DataProvider { get; set; } = new FileDataProvider();

        public IEnumerable<string> GetNames(string lang)
        {
            var indexData = DataProvider.GetData(IndexFilePath);
            var indexNode = JsonPath.Node.FromYaml(indexData, new YamlDeserializer.Options { LowerCaseDictKeys = true });
            return indexNode["entries"]
                .AsDictionary
                .Where(kv => Is.Value(kv.Value[lang]))
                .Select(kv => kv.Key);
        }

        public TimelineEntry GetEntry(string name, string lang)
        {
            var id = Timeline.GetNameFromSeoTitle(name);

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
