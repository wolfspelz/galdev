using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace galdevtool
{
    public class Yaml2Bigfile
    {
        public ICallbackLogger Log { get; set; } = new NullCallbackLogger();
        public ICallbackConfig Config { get; set; } = new MemoryCallbackConfig();

        public string InputFolderPath => (string)Config.Get(nameof(AppConfig.Yaml2BigfileInputFolderPath), "");
        public string OutputFilePath => (string)Config.Get(nameof(AppConfig.Yaml2BigfileOutputYamlFilePath), "");
        public string OutputImageFolderPath => (string)Config.Get(nameof(AppConfig.Yaml2BigfileOutputImagePath), "");
        public string OutputPostImageFolderPath => (string)Config.Get(nameof(AppConfig.Yaml2BigfileOutputSnImagePath), "");
        public string YamlImageFolderName => (string)Config.Get(nameof(AppConfig.YamlImageFolderName), "");

        public void Convert()
        {
            Log.Info("");
            var entries = Read(InputFolderPath);
            Write(entries, OutputFilePath, Path.Combine(InputFolderPath, YamlImageFolderName), OutputImageFolderPath, OutputPostImageFolderPath);
        }

        public List<TimelineEntry> Read(string inputFolder)
        {
            var timeline = new List<TimelineEntry>();

            foreach (var file in Directory.EnumerateFiles(inputFolder, "*.yaml", SearchOption.TopDirectoryOnly).OrderBy(x => x))
            {
                Log.Info(Path.GetFileName(file));
                if (!file.Contains("_")) continue;
                var data = File.ReadAllText(file);

                //var yaml = new SharpYaml.Serialization.Serializer().Deserialize(data);

                var deserializer = new YamlDotNet.Serialization.Deserializer();
                var dict = deserializer.Deserialize<Dictionary<string, object>>(data);

                var e = new TimelineEntry();
                foreach (var pair in dict)
                {
                    switch (pair.Key.ToLower())
                    {
                        case "name": e.Name = (string)pair.Value; break;
                        case "year": e.Year = (string)pair.Value; break;
                        case "title": e.Title = (string)pair.Value; break;
                        case "short": e.Short = (string)pair.Value; break;
                        case "summary": e.Summary = (string)pair.Value; break;
                        case "image": e.Image = (string)pair.Value; break;
                        case "smallimage": e.Smallimage = (string)pair.Value; break;
                        case "headline": e.Headline = (string)pair.Value; break;
                        case "post": e.Post = (string)pair.Value; break;
                        case "postimage": e.Postimage = (string)pair.Value; break;
                        case "twitter": e.Twitter = (string)pair.Value; break;
                        case "twitterimage": e.Twitterimage = (string)pair.Value; break;
                        case "facebook": e.Facebook = (string)pair.Value; break;
                        case "facebook2": e.Facebook2 = (string)pair.Value; break;
                        case "facebook3": e.Facebook3 = (string)pair.Value; break;
                        case "facebookimage": e.Facebookimage = (string)pair.Value; break;
                        case "tags": e.Tags = ((List<object>)pair.Value).Select(o => (string)o).ToList(); break;
                        case "topics": e.Topics = ((List<object>)pair.Value).Select(o => (string)o).ToList(); break;
                        case "text": e.Text = ((string)pair.Value).Replace("\r\n", "\n").Split(new char[] { '\n' }).ToList(); break;
                    }
                }
                timeline.Add(e);
            }

            return timeline;
        }

        public void Write(List<TimelineEntry> entries, string outputFile, string inputImgFolder, string outputImgFolder, string outputSnImgFolder)
        {
            var sb = new StringBuilder();

            foreach (var e in entries)
            {
                sb.Append(e.Year);
                sb.Append('\t');
                sb.Append(e.Title);
                sb.Append('.');
                if (e.Text.Count > 0)
                {
                    sb.Append(" # ");
                    sb.Append(string.Join(" # ", e.Text.Where(x => !string.IsNullOrEmpty(x))));
                }
                if (e.Tags.Count > 0)
                {
                    sb.Append(" | tags=");
                    sb.Append(string.Join(" ", e.Tags.Select(x => "#" + x)));
                }
                if (!string.IsNullOrEmpty(e.Image))
                {
                    sb.Append(" | image=");
                    sb.Append(e.Image);
                }
                if (!string.IsNullOrEmpty(e.Smallimage))
                {
                    sb.Append(" | smallimage=");
                    sb.Append(e.Smallimage);
                }
                if (!string.IsNullOrEmpty(e.Twitter))
                {
                    sb.Append(" | twitter=");
                    sb.Append(e.Twitter);
                }
                if (!string.IsNullOrEmpty(e.Twitterimage))
                {
                    sb.Append(" | twitterimage=");
                    sb.Append(e.Twitterimage);
                }
                if (!string.IsNullOrEmpty(e.Facebook))
                {
                    sb.Append(" | facebook=");
                    if (!string.IsNullOrEmpty(e.Headline))
                    {
                        sb.Append(e.Headline);
                        sb.Append(" - ");
                    }
                    sb.Append(e.Facebook);
                    if (!string.IsNullOrEmpty(e.Facebook2))
                    {
                        sb.Append(" # ");
                        sb.Append(e.Facebook2);
                    }
                    if (!string.IsNullOrEmpty(e.Facebook3))
                    {
                        sb.Append(" # ");
                        sb.Append(e.Facebook3);
                    }
                }
                if (!string.IsNullOrEmpty(e.Facebookimage))
                {
                    sb.Append(" | facebookimage=");
                    sb.Append(e.Facebookimage);
                }
                if (e.Topics.Count > 0)
                {
                    sb.Append(" | topic=");
                    sb.Append(string.Join(",", e.Topics));
                }
                sb.Append("\r\n");

                if (!string.IsNullOrEmpty(e.Image))
                {
                    var src = Path.Combine(inputImgFolder, e.Image);
                    var dst = Path.Combine(OutputImageFolderPath, e.Image);
                    CopyFile(src, dst);
                }

                if (!string.IsNullOrEmpty(e.Smallimage))
                {
                    var src = Path.Combine(inputImgFolder, e.Smallimage);
                    var dst = Path.Combine(OutputImageFolderPath, e.Smallimage);
                    CopyFile(src, dst);
                }

                if (!string.IsNullOrEmpty(e.Twitterimage))
                {
                    var src = Path.Combine(inputImgFolder, e.Twitterimage);
                    var dst = Path.Combine(OutputPostImageFolderPath, e.Twitterimage);
                    CopyFile(src, dst);
                }

                if (!string.IsNullOrEmpty(e.Facebookimage))
                {
                    var src = Path.Combine(inputImgFolder, e.Facebookimage);
                    var dst = Path.Combine(OutputPostImageFolderPath, e.Facebookimage);
                    CopyFile(src, dst);
                }
            }

            File.WriteAllText(outputFile, sb.ToString());
        }

        private static void CopyFile(string src, string dst)
        {
            if (File.Exists(src))
            {
                PrepareFolder(Path.GetDirectoryName(dst), 0);
                File.Copy(src, dst, overwrite: true);
            }
        }

        private static void PrepareFolder(string path, int recursion)
        {
            if (recursion > 1) return;
            if (!Directory.Exists(path))
            {
                PrepareFolder(Path.GetDirectoryName(path), recursion + 1);
                Directory.CreateDirectory(path);
            }
        }
    }
}
