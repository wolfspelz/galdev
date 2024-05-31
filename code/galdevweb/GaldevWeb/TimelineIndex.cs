namespace GaldevWeb
{
    public delegate bool TimelineEntryCondition(TimelineEntry entry);

    public class TimelineIndex
    {
        public IDataProvider DataProvider = new TabConvertingFileDataProvider();
        public string IndexFilePath = "timeline-index.yaml";
        public ICallbackLogger Log = new NullCallbackLogger();

        private readonly Dictionary<string, TimelineLanguage> _languageById = new();
        private readonly Dictionary<string, TimelineSeries> _timelineByLang = new();

        public TimelineSeries GetSeriesForLanguage(string lang) => _timelineByLang[lang];

        private TimelineEntryCondition _lastFilter = (e) => true;

        public void Load(TimelineEntryCondition filter)
        {
            _lastFilter = filter;

            LoadLanguages();
            LoadEntries(filter);
            LoadSequences();
            ConnectEntries();
            CreateTopics();
            CreateAliases();
        }

        public void Reload()
        {
            Unload();
            Load(_lastFilter);
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

        private void LoadEntries(TimelineEntryCondition filter)
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
                    if (entry != null) {
                        if (filter == null || filter(entry)) {
                            _timelineByLang[lang].Add(name, entry);
                        }
                    }
                }

            }
        }

        private void LoadSequences()
        {
            var indexData = DataProvider.GetData(IndexFilePath);
            var indexNode = JsonPath.Node.FromYaml(indexData, new YamlDeserializer.Options { LowerCaseDictKeys = true });

            var sequencesNode = indexNode["sequences"].AsDictionary;
            foreach (var sequenceNode in sequencesNode) {
                var name = sequenceNode.Key;

                foreach (var threadByLang in sequenceNode.Value.AsDictionary) {
                    var lang = threadByLang.Key;
                    var title = threadByLang.Value.AsDictionary["title"].AsString;
                    var summary = threadByLang.Value.AsDictionary["summary"].AsString;
                    var entries = threadByLang.Value.AsDictionary["entries"].AsList.Select(x => x.String);
                    var sequence = new TimelineSequence(title, summary, entries);
                    _timelineByLang[lang].AddSequence(name, sequence);
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
            var fileNameWithExt = fileName + ".yaml";

            if (!DataProvider.HasData(entryPathWithExt)) {
                entryPathWithExt = entryPath + ".yml";
                fileNameWithExt = fileName + ".yml";
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

            var entry = new TimelineEntry(entryName, year, title, fileNameWithExt, text);

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

            var sequences = contentNode["sequencetext"].AsDictionary;
            foreach (var sequence in sequences) {
                entry._sequenceText[sequence.Key] = sequence.Value.AsString;
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
