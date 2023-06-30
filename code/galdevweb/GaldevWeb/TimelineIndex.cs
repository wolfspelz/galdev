using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace GaldevWeb
{
    public class TimelineIndex
    {
        public IDataProvider DataProvider = new FileDataProvider();
        public string IndexFilePath = "timeline/index.yaml";

        private Dictionary<string, TimelineLanguage> _languageById = new Dictionary<string, TimelineLanguage>();
        private Dictionary<string, TimelineSeries> _timelineByLang = new Dictionary<string, TimelineSeries>();

        public delegate bool TimelineEntryCondition(TimelineEntry entry);
        public TimelineSeries GetSeriesForLanguage(string lang) => _timelineByLang[lang];
        public TimelineSeries GetSeriesForLanguageWithFilter(string lang, TimelineEntryCondition filter)
        {
            var entries = _timelineByLang[lang].Where(kv => filter(kv.Value));
            return new TimelineSeries(entries);
        }

        public void Load()
        {
            var indexData = DataProvider.GetData(IndexFilePath);
            var indexNode = JsonPath.Node.FromYaml(indexData, new YamlDeserializer.Options { LowerCaseDictKeys = true });

            LoadLanguages(indexNode);
            LoadEntries(indexNode);
            ConnectEntries();
        }

        private void ConnectEntries()
        {
            foreach (var kv in _timelineByLang) {
                var timeline = kv.Value;
                timeline.ConnectEntries();
            }
        }

        private void LoadLanguages(Node indexNode)
        {
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

        private void LoadEntries(Node indexNode)
        {
            foreach (var kv in _languageById) {
                _timelineByLang.Add(kv.Key, new TimelineSeries());
            }

            var entriesNode = indexNode["entries"].AsDictionary;
            foreach (var entryNode in entriesNode) {
                var name = entryNode.Key;
                foreach (var fileByLang in entryNode.Value.AsDictionary) {
                    var lang = fileByLang.Key;
                    var langPath = _languageById[lang].TextPath;

                    var entryFileName = fileByLang.Value.AsString;
                    if (entryFileName.EndsWith(".yaml")) {
                        entryFileName = entryFileName.Substring(0, entryFileName.Length - ".yaml".Length);
                    }
                    if (entryFileName.EndsWith(".yml")) {
                        entryFileName = entryFileName.Substring(0, entryFileName.Length - ".yml".Length);
                    }

                    var dirPath = Path.GetDirectoryName(IndexFilePath);
                    dirPath = dirPath ?? "";
                    var entryPath = (dirPath + "/" + langPath + "/" + entryFileName).Replace("//", "/");
                    var entryPathWithExt = entryPath + ".yaml";

                    if (!DataProvider.HasData(entryPathWithExt)) {
                        entryPathWithExt = entryPath + ".yml";
                        if (!DataProvider.HasData(entryPathWithExt)) {
                            throw new Exception($"No entry file= {entryPathWithExt}");
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

                    var entry = new TimelineEntry(name, year, title, text);

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

                    var image = contentNode["image"].AsString;
                    if (Is.Value(image)) {
                        entry.Image = $"{lang}/{image}";
                    }

                    _timelineByLang[lang].Add(name, entry);
                }
            }
        }

        public TimelineEntry GetEntry(string name, string lang)
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

        public string GetImagePath(string imageName, string lang)
        {
            var indexData = DataProvider.GetData(IndexFilePath);
            var indexNode = JsonPath.Node.FromYaml(indexData, new YamlDeserializer.Options { LowerCaseDictKeys = true });
            var langPath = indexNode["languages"][lang]["images"].AsString;
            return Path.Combine(Path.GetDirectoryName(IndexFilePath) ?? "", langPath, imageName);
        }

    }
}
