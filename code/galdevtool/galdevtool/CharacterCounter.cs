using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace galdevtool
{
    public class CharacterCounter
    {
        public ICallbackLogger Log { get; set; } = new NullCallbackLogger();
        public ICallbackConfig Config { get; set; } = new MemoryCallbackConfig();

        public string DataFolderPath => (string)Config.Get(nameof(AppConfig.CountCharactersDataFolderPath), "");

        public void LogCounts()
        {
            Log.Info("");
            var inputFiles = Read(DataFolderPath);
            var entries = ProcessInput(inputFiles);
            LogLines(entries);
        }

        public Dictionary<string, string> Read(string inputFolder)
        {
            var listOfYamlData = new Dictionary<string, string>();

            foreach (var file in Directory.EnumerateFiles(inputFolder, "*.yaml", SearchOption.TopDirectoryOnly).OrderBy(x => x))
            {
                Log.Info(Path.GetFileName(file));
                if (!file.Contains("_")) continue;
                var yaml = File.ReadAllText(file);
                listOfYamlData.Add(file, yaml);
            }

            return listOfYamlData;
        }

        public List<TimelineEntry> ProcessInput(Dictionary<string, string> inputData)
        {
            var timeline = new List<TimelineEntry>();

            foreach (var filePair in inputData)
            {
                Log.Info(Path.GetFileName(filePair.Key));

                var deserializer = new YamlDotNet.Serialization.Deserializer();
                var dict = deserializer.Deserialize<Dictionary<string, object>>(filePair.Value);

                var e = new TimelineEntry();
                foreach (var linePair in dict)
                {
                    switch (linePair.Key.ToLower())
                    {
                        case "name": e.Name = (string)linePair.Value; break;
                        case "year": e.Year = (string)linePair.Value; break;
                        case "title": e.Title = (string)linePair.Value; break;
                        case "short": e.Short = (string)linePair.Value; break;
                        case "summary": e.Summary = (string)linePair.Value; break;
                        case "image": e.Image = (string)linePair.Value; break;
                        case "smallimage": e.Smallimage = (string)linePair.Value; break;
                        case "headline": e.Headline = (string)linePair.Value; break;
                        case "post": e.Post = (string)linePair.Value; break;
                        case "postimage": e.Postimage = (string)linePair.Value; break;
                        case "twitter": e.Twitter = (string)linePair.Value; break;
                        case "twitterimage": e.Twitterimage = (string)linePair.Value; break;
                        case "facebook": e.Facebook = (string)linePair.Value; break;
                        case "facebook2": e.Facebook2 = (string)linePair.Value; break;
                        case "facebook3": e.Facebook3 = (string)linePair.Value; break;
                        case "facebookimage": e.Facebookimage = (string)linePair.Value; break;
                        case "author": e.Author = (string)linePair.Value; break;
                        case "translation": e.Translation = (string)linePair.Value; break;
                        case "tags": e.Tags = ((List<object>)linePair.Value).Select(o => (string)o).ToList(); break;
                        case "topics": e.Topics = ((List<object>)linePair.Value).Select(o => (string)o).ToList(); break;
                        case "text": e.Text = ((string)linePair.Value).Replace("\r\n", "\n").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.TrimEnd()).ToList(); break;
                    }

                    if (string.IsNullOrEmpty(e.Post)) { e.Postimage = ""; }
                    if (string.IsNullOrEmpty(e.Twitter)) { e.Twitterimage = ""; }
                    if (string.IsNullOrEmpty(e.Facebook)) { e.Facebookimage = ""; }
                }
                timeline.Add(e);
            }

            return timeline;
        }

        public void LogLines(List<TimelineEntry> entries)
        {
            foreach (var e in entries) {
                var len = string.Join(" ", e.Text.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimEnd())).Length;
                Log.User($"{len}\t{e.Year}\t{e.Title}");
            }
        }
    }
}
