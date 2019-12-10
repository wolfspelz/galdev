using System.Collections.Generic;
using System.IO;

namespace galdevtool
{
    public class Yaml2Bigfile
    {
        public ICallbackLogger Log { get; set; } = new NullCallbackLogger();
        public ICallbackConfig Config { get; set; } = new MemoryCallbackConfig();

        public string InputFolderPath => (string)Config.Get(nameof(AppConfig.Yaml2BigfileYamlFolderPath), "");
        public string OutputFilePath => (string)Config.Get(nameof(AppConfig.Yaml2BigfileFilePath), "");
        public string OutputImageFolderPath => (string)Config.Get(nameof(AppConfig.Yaml2BigfileImagePath), "");
        public string OutputPostImageFolderPath => (string)Config.Get(nameof(AppConfig.Yaml2BigfileSnImagePath), "");

        public void Convert()
        {
            Log.Info("");
            var entries = Read(InputFolderPath);
            Write(entries, OutputFilePath, OutputImageFolderPath, OutputPostImageFolderPath);
        }

        public List<TimelineEntry> Read(string inputFolder)
        {
            var timeline = new List<TimelineEntry>();

            foreach (var file in Directory.EnumerateFiles(inputFolder, "*.yaml", SearchOption.TopDirectoryOnly))
            {
                Log.Info(Path.GetFileName(file));
                if (!file.Contains("_")) continue;
                var data = File.ReadAllText(file);

                //var yaml = new SharpYaml.Serialization.Serializer().Deserialize(data);

                //var deserializer = new YamlDotNet.Serialization.Deserializer();
                //var dict = deserializer.Deserialize<Dictionary<string, string>>(data);

                var e = new TimelineEntry();
                //foreach (var pair in dict)
                //{
                //    switch (pair.Key)
                //    {
                //        case "name": e.Name = pair.Value; break;
                //        case "year": e.Year = pair.Value; break;
                //        case "title": e.Title = pair.Value; break;
                //        case "short": e.Short = pair.Value; break;
                //        case "summary": e.Summary = pair.Value; break;
                //        case "image": e.Image = pair.Value; break;
                //        case "headline": e.Headline = pair.Value; break;
                //        case "smallimage": e.Smallimage = pair.Value; break;
                //        case "post": e.Post = pair.Value; break;
                //        case "postimage": e.Postimage = pair.Value; break;
                //        case "twitter": e.Twitter = pair.Value; break;
                //        case "twitterimage": e.Twitterimage = pair.Value; break;
                //        case "facebook": e.Facebook = pair.Value; break;
                //        case "facebookimage": e.Facebookimage = pair.Value; break;
                //        //case "tags": e.Tags = pair.Value.Select; break;
                //        //case "topics": e.Topics = pair.Value; break;
                //        //case "text": e.Text = pair.Value; break;
                //    }
                //}
            }

            return timeline;
        }

        public void Write(List<TimelineEntry> entries, string outputFolder, string imgFolder, string snImgFolder)
        {
        }

    }
}
