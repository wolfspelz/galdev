﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace galdevtool
{
    public class Bigfile2Yaml
    {
        public ICallbackLogger Log { get; set; } = new NullCallbackLogger();
        public ICallbackConfig Config { get; set; } = new MemoryCallbackConfig();

        public string InputFilePath => (string)Config.Get(nameof(AppConfig.BigfilePath), "");
        public string OutputFolderPath => (string)Config.Get(nameof(AppConfig.YamlFolderPath), "");
        public string InputImageFolderPath => (string)Config.Get(nameof(AppConfig.ImagePath), "");
        public string InputPostImageFolderPath => (string)Config.Get(nameof(AppConfig.SnImagePath), "");

        public string Read(string inputFile)
        {
            var data = File.ReadAllText(inputFile);
            return data;
        }

        public void Convert()
        {
            Log.Info("");
            var data = Read(InputFilePath);
            var entries = Analyse(data);
            Write(entries, OutputFolderPath, InputImageFolderPath, InputPostImageFolderPath);
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
                                        if (claimAndText.Contains(" - "))
                                        {
                                            var claimAndTextParts = claimAndText.Split(new char[] { '-' }, 2);
                                            if (claimAndTextParts.Length > 0)
                                            {
                                                e.Headline = claimAndTextParts[0].Trim();
                                            }
                                            if (claimAndTextParts.Length > 1)
                                            {
                                                var x = claimAndTextParts[1].Trim();
                                                var pos = x.IndexOf("(");
                                                if (pos > 0)
                                                {
                                                    x = x.Substring(0, pos).Trim();
                                                }
                                                e.Facebook = x;
                                            }
                                        }
                                        else
                                        {
                                            var x = claimAndText;
                                            var pos = x.IndexOf("(");
                                            if (pos > 0)
                                            {
                                                x = x.Substring(0, pos).Trim();
                                            }
                                            e.Facebook = x;
                                        }
                                        if (fbParts.Length == 3)
                                        {
                                            var x = fbParts[2].Trim();
                                            var pos = x.ToLower().IndexOf("weiter");
                                            if (pos > 0)
                                            {
                                                x = x.Substring(0, pos).Trim();
                                            }
                                            e.Short = x;
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

        public void Write(List<TimelineEntry> entries, string outputFolder, string imgFolder, string snImgFolder)
        {
            var outImgFolder = "images";
            Directory.CreateDirectory(Path.Combine(outputFolder, outImgFolder));

            var index = @"images: ./{outImgFolder}/
topics:
  accident: Unfälle und Havarien
  adventure: Abenteuer
  ai: Künstliche Intelligenz
  aliens: Völker
  catastrophe: Katastrophen
  conspiracy: Verschwörungen
  crime: Verbrechen
  culture: Kultur
  discovery: Entdeckungen
  ecology: Ökologie, Biologie und Umwelt
  economy: Wirtschaft und Wirtschaftskrisen
  epidemic: Epidemien
  event: Aufregende Ereignisse
  luck: Mehr Glück als Verstand
  people: Personen
  philosophy: Philosophie und Religion
  politics: Politik
  science: Wissenschaft
  spaceflight: Raumfahrt
  statistics: Statistik
  technology: Technik
  things: Gegenstände
  upgrades: Verbesserungen für Menschen
  visitors: Aliens im Sonnensystem
  war: Kriege
  wonder: Wunder
timeline: 
";
            var indexLabels = new HashSet<string>();
            foreach (var e in entries)
            {
                var indexLabel = e.Year;
                var cnt = 0;
                while (indexLabels.Contains(indexLabel))
                {
                    indexLabel = e.Year + (char)('a' + cnt);
                    cnt++;
                }
                indexLabels.Add(indexLabel);

                var name = "";
                if (string.IsNullOrEmpty(name))
                {
                    name = e.Image;
                    if (!string.IsNullOrEmpty(name))
                    {
                        var dot = name.IndexOf('.');
                        if (dot > 2)
                        {
                            name = name.Substring(0, dot);
                        }
                    }
                }
                if (string.IsNullOrEmpty(name))
                {
                    name = e.Title
                    .Replace(" ", "")
                    .Replace("/", "")
                    .Replace(":", "")
                    .Replace("-", "")
                    .Replace(",", "")
                    .Replace("(", "")
                    .Replace(")", "")
                    .Replace("\"", "")
                    ;
                    var dot = name.IndexOf('.');
                    if (dot > 0)
                    {
                        name = name.Substring(0, dot);
                    }
                    name = name.Truncate(40, "");
                }
                index += $"  - {indexLabel}: {name}\r\n";

                if (!string.IsNullOrEmpty(e.Image))
                {
                    var src = Path.Combine(imgFolder, e.Image);
                    var dst = Path.Combine(outputFolder, outImgFolder, e.Image);
                    if (File.Exists(src))
                    {
                        File.Copy(src, dst);
                    }
                    else
                    {
                        e.Image = "";
                    }
                }

                if (!string.IsNullOrEmpty(e.Facebookimage))
                {
                    var src = Path.Combine(snImgFolder, e.Facebookimage);
                    var dst = Path.Combine(outputFolder, outImgFolder, e.Facebookimage);
                    if (File.Exists(src))
                    {
                        File.Copy(src, dst, true);
                        e.Postimage = e.Facebookimage;
                    }
                    else
                    {
                        e.Facebookimage = "";
                    }
                }

                if (!string.IsNullOrEmpty(e.Twitterimage))
                {
                    var src = Path.Combine(snImgFolder, e.Twitterimage);
                    var dst = Path.Combine(outputFolder, outImgFolder, e.Twitterimage);
                    if (File.Exists(src))
                    {
                        File.Copy(src, dst, true);
                        e.Postimage = e.Twitterimage; // larger than FB image
                    }
                    else
                    {
                        e.Twitterimage = "";
                    }
                }

                e.Post = e.Facebook;

                var entry = "";
                entry += $"year: {e.Year}\r\n";
                entry += $"title: {e.Title}\r\n";
                if (!string.IsNullOrEmpty(e.Short)) { entry += $"short: {e.Short}\r\n"; }
                if (!string.IsNullOrEmpty(e.Summary)) { entry += $"summary: {e.Summary}\r\n"; }
                if (!string.IsNullOrEmpty(e.Headline)) { entry += $"headline: {e.Headline}\r\n"; }
                if (!string.IsNullOrEmpty(e.Image)) { entry += $"image: {e.Image}\r\n"; }
                if (!string.IsNullOrEmpty(e.Smallimage)) { entry += $"smallimage: {e.Smallimage}\r\n"; }
                if (!string.IsNullOrEmpty(e.Post)) { entry += $"post: {e.Post}\r\n"; }
                if (!string.IsNullOrEmpty(e.Postimage)) { entry += $"postimage: {e.Postimage}\r\n"; }
                if (!string.IsNullOrEmpty(e.Facebook)) { entry += $"facebook: {e.Facebook}\r\n"; }
                if (!string.IsNullOrEmpty(e.Facebookimage)) { entry += $"facebookimage: {e.Facebookimage}\r\n"; }
                if (!string.IsNullOrEmpty(e.Twitter)) { entry += $"twitter: {e.Twitter}\r\n"; }
                if (!string.IsNullOrEmpty(e.Twitterimage)) { entry += $"twitterimage: {e.Twitterimage}\r\n"; }
                if (e.Tags.Count > 0) { entry += $"tags:\r\n{string.Join("\r\n", e.Tags.Select(x => "  - " + x))}\r\n"; }
                if (e.Topics.Count > 0) { entry += $"topics:\r\n{string.Join("\r\n", e.Topics.Select(x => "  - " + x))}\r\n"; }
                if (e.Text.Count > 0) { entry += $"text: |  \r\n{string.Join("\r\n", e.Text.Select(x => "  " + x))}\r\n"; }

                File.WriteAllText(Path.Combine(outputFolder, name + ".yaml"), entry);
            }

            File.WriteAllText(Path.Combine(outputFolder, "index.yaml"), index);
        }

    }
}