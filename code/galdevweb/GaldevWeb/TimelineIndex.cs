namespace GaldevWeb
{
    public class TimelineIndex
    {
        public IDataProvider DataProvider = new TabConvertingFileDataProvider();
        public string IndexFilePath = "timeline-index.yaml";
        public ICallbackLogger Log = new NullCallbackLogger();

        private readonly Dictionary<string, TimelineLanguage> _languageById = new();
        private readonly Dictionary<string, TimelineSeries> _timelineByLang = new();

        public TimelineSeries GetSeriesForLanguage(string lang) => _timelineByLang[lang];

        public void Load()
        {
            LoadLanguages();
            LoadEntries();
            ConnectEntries();
            CreateTopics();
            CreateAliases();
        }

        public void Reload()
        {
            Unload();
            Load();
        }

        private void Unload()
        {
            _languageById.Clear();
            _timelineByLang.Clear();
        }

        private void CreateAliases()
        {
            foreach (var kv in _timelineByLang) {
                var timeline = kv.Value;
                timeline.CreateAliases();
            }
        }

        private void CreateTopics()
        {
            foreach (var kv in _timelineByLang) {
                var timeline = kv.Value;
                var langageConfig = _languageById[kv.Key];
                timeline.CreateTopics(langageConfig.Topics);
            }
        }

        private void ConnectEntries()
        {
            foreach (var kv in _timelineByLang) {
                var timeline = kv.Value;
                timeline.ConnectEntries();
            }
        }

        private void LoadLanguages()
        {
            var indexData = DataProvider.GetData(IndexFilePath);
            var indexNode = JsonPath.Node.FromYaml(indexData, new YamlDeserializer.Options { LowerCaseDictKeys = true });

            var languagesNode = indexNode["languages"].AsDictionary;
            foreach (var langNode in languagesNode) {
                var id = langNode.Key;
                var path = langNode.Value["path"].AsString;
                var imagePath = langNode.Value["images"].AsString;
                var topics = langNode.Value["topics"].AsDictionary.ToDictionary(kv => kv.Key, kv => kv.Value.AsString);
                var languageConfig = new TimelineLanguage(id, path, imagePath, topics);
                _languageById.Add(id, languageConfig);
            }
        }

        private void LoadEntries()
        {
            var indexData = DataProvider.GetData(IndexFilePath);
            var indexNode = JsonPath.Node.FromYaml(indexData, new YamlDeserializer.Options { LowerCaseDictKeys = true });

            foreach (var kv in _languageById) {
                _timelineByLang.Add(kv.Key, new TimelineSeries());
            }

            var entriesNode = indexNode["entries"].AsDictionary;
            foreach (var entryNode in entriesNode) {
                var name = entryNode.Key;
                foreach (var fileByLang in entryNode.Value.AsDictionary) {
                    var lang = fileByLang.Key;
                    var folderPath = _languageById[lang].TextPath;
                    var fileName = fileByLang.Value.AsString;

                    TimelineEntry? entry = GetEntryFromFile(lang, name, folderPath, fileName);
                    if (entry != null && !entry.Tags.Contains("_hidden")) {
                        _timelineByLang[lang].Add(name, entry);
                    }
                }
            }
        }

        private TimelineEntry? GetEntryFromFile(string lang, string entryName, string folderPath, string fileName)
        {
            if (fileName.EndsWith(".yaml")) {
                fileName = fileName[..^".yaml".Length];
            }
            if (fileName.EndsWith(".yml")) {
                fileName = fileName[..^".yml".Length];
            }

            var dirPath = Path.GetDirectoryName(IndexFilePath);
            dirPath ??= "";
            var entryPath = (dirPath + "/" + folderPath + "/" + fileName).Replace("//", "/");
            var entryPathWithExt = entryPath + ".yaml";

            if (!DataProvider.HasData(entryPathWithExt)) {
                entryPathWithExt = entryPath + ".yml";
                if (!DataProvider.HasData(entryPathWithExt)) {
                    Log.Warning($"No entry file= {entryPathWithExt}");
                    return null;
                }
            }

            var contentData = DataProvider.GetData(entryPathWithExt);
            var contentNode = JsonPath.Node.FromYaml(contentData, new YamlDeserializer.Options { LowerCaseDictKeys = true });

            var year = contentNode["year"].AsString;
            var title = contentNode["title"].AsString;
            var text = contentNode["text"].AsString
                .Replace("\r\n", "\n")
                .Replace("\r", "\n")
                .Replace("\n\n", "\n")
                .Split('\n')
                .Select(x => x.Trim())
                .ToArray();

            var entry = new TimelineEntry(entryName, year, title, text);

            var shortTitle = contentNode["shorttitle"].AsString;
            if (Is.Value(shortTitle)) {
                entry.ShortTitle = shortTitle;
            }

            var summary = contentNode["summary"].AsString;
            if (Is.Value(summary)) {
                entry.Summary = summary;
            }

            var shortSummary = contentNode["short"].AsString;
            if (Is.Value(shortSummary)) {
                entry.Summary = shortSummary;
            }

            var headline = contentNode["headline"].AsString;
            if (Is.Value(headline)) {
                entry.Headline = headline;
            }

            var image = contentNode["image"].AsString;
            if (Is.Value(image) && !image.Contains("NAME")) {
                entry.Image = $"{lang}/{image}";
            }

            entry.Topics = contentNode["topics"].AsList.Select(n => n.AsString).ToArray();
            entry.Aliases = contentNode["aliases"].AsList.Select(n => n.AsString).ToArray();
            entry.Tags = contentNode["tags"].AsList.Select(n => n.AsString).ToArray();

            return entry;
        }

        public TimelineEntry GetEntry(string lang, string name)
        {
            var timeline = _timelineByLang[lang];
            var entry = timeline[name];
            return entry;
        }

        public static string GetNameFromSeoTitle(string seoTitle)
        {
            var parts = seoTitle.ToLower().Split(new char[] { '-', ':' }, 2);
            return parts[0];
        }

        public string GetImagePath(string lang, string imageName)
        {
            var indexData = DataProvider.GetData(IndexFilePath);
            var indexNode = JsonPath.Node.FromYaml(indexData, new YamlDeserializer.Options { LowerCaseDictKeys = true });
            var langPath = indexNode["languages"][lang]["images"].AsString;
            return Path.Combine(Path.GetDirectoryName(IndexFilePath) ?? "", langPath, imageName);
        }

    }
}
