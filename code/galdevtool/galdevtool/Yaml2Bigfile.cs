using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace galdevtool
{
    public class Yaml2Bigfile
    {
        public ICallbackLogger Log { get; set; } = new NullCallbackLogger();
        public ICallbackConfig Config { get; set; } = new MemoryCallbackConfig();

        public string InputFolderPath => (string)Config.Get(nameof(AppConfig.YamlFolderPath), "");
        public string OutputFilePath => (string)Config.Get(nameof(AppConfig.BigfilePath), "");
        public string OutputImageFolderPath => (string)Config.Get(nameof(AppConfig.ImagePath), "");
        public string OutputPostImageFolderPath => (string)Config.Get(nameof(AppConfig.SnImagePath), "");

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
                if (!file.Contains("_")) continue;
                var data = File.ReadAllText(file);
                var deserializer = new YamlDotNet.Serialization.Deserializer();
                var dict = deserializer.Deserialize<Dictionary<string, string>>(data);
            }

            return timeline;
        }

        public void Write(List<TimelineEntry> entries, string outputFolder, string imgFolder, string snImgFolder)
        {
        }

    }
}
