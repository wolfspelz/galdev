using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace galdevtool
{
    public class Bigfile2Yaml
    {
        public ICallbackLogger Log { get; set; } = new NullCallbackLogger();

        public string Read(string inputFile)
        {
            var data = File.ReadAllText(inputFile);
            return data;
        }

        public void Convert(string inputFile, string outputFolder)
        {
            Log.Info("");
            var data = Read(inputFile);
            var entries = Analyse(data);
            Write(entries, outputFolder);
        }

        public List<TimelineEntry> Analyse(string data)
        {
            var timeline = new List<TimelineEntry>();

            var lines = data.Replace("\r\n", "\n").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var lineCount = 0;
            foreach (var line in lines)
            {
                var e = new TimelineEntry();

                var segments = line.Split(new char[] { '\t' }, 2);
                e.Year = segments[0];
                var text = segments[1];

                var sections = segments[1].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (sections.Length > 0)
                {
                    var paragraphs = sections[0].Split(new char[] { '#' });
                    if (paragraphs.Length > 0)
                    {
                        e.Title = paragraphs[0].Trim().Trim('.');
                    }
                    if (paragraphs.Length > 1)
                    {
                        for (var i = 1; i < paragraphs.Length; i++)
                        {
                            e.Text.Add(paragraphs[i].Trim());
                        }
                    }
                }

                if (sections.Length > 1)
                {
                    for (var i = 1; i < sections.Length; i++)
                    {
                        var parts = sections[i].Trim().Split(new char[] { '=' }, 2); ;
                        if (parts.Length == 1)
                        {
                            e.Tags = parts[0].Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Replace("#", "").Trim()).ToList();
                        }
                        if (parts.Length == 2)
                        {
                            var key = parts[0].Trim();
                            var value = parts[1].Trim();
                            switch (key)
                            {
                                case "tags":
                                    e.Tags = value.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Replace("#", "").Trim()).ToList();
                                    break;
                                case "image":
                                    e.Image = value;
                                    break;
                                case "twitter":
                                    e.Twitter = value;
                                    break;
                                case "twitterimage":
                                    e.Twitterimage = value;
                                    break;
                                case "facebook":
                                    {
                                        var fbParts = value.Split(new char[] { '#' });
                                        var claimAndText = fbParts[0].Trim();
                                        var claimAndTextParts = claimAndText.Split(new char[] { '-' });
                                        if (claimAndTextParts.Length > 0)
                                        {
                                            e.Headline = claimAndTextParts[0].Trim();
                                        }
                                        if (claimAndTextParts.Length > 1) 
                                        {
                                            var x = claimAndTextParts[1].Trim();
                                            var pos = x.IndexOf("(");
                                            var y = x.Substring(0, pos).Trim();
                                            e.Facebook = y;
                                        }
                                        if (fbParts.Length == 3)
                                        {
                                            var x = fbParts[2].Trim();
                                            var pos = x.ToLower().IndexOf("weiter");
                                            var y = x.Substring(0, pos).Trim();
                                            e.Short = y;
                                        }
                                    }
                                    break;
                                case "facebookimage":
                                    e.Facebookimage = value;
                                    break;
                                case "topic":
                                    e.Topics = value.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                timeline.Add(e);
                lineCount++;
            }

            return timeline;
        }

        public void Write(List<TimelineEntry> entries, string outputFolder)
        {
            throw new NotImplementedException();
        }

    }
}
