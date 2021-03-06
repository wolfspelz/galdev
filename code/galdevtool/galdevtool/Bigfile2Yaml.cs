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

        public string InputFilePath => (string)Config.Get(nameof(AppConfig.Bigfile2YamlInputYamlFilePath), "");
        public string OutputFolderPath => (string)Config.Get(nameof(AppConfig.Bigfile2YamlOutputFolderPath), "");
        public string InputImageFolderPath => (string)Config.Get(nameof(AppConfig.Bigfile2YamlInputImageFolderPath), "");
        public string InputPostImageFolderPath => (string)Config.Get(nameof(AppConfig.Bigfile2YamlInputSnImagePath), "");
        public string YamlImageFolderName => (string)Config.Get(nameof(AppConfig.YamlImageFolderName), "");

        public void Convert()
        {
            Log.Info("");
            var data = Read(InputFilePath);
            var entries = Analyse(data);
            Write(entries, OutputFolderPath, InputImageFolderPath, InputPostImageFolderPath);
        }

        public string Read(string inputFile)
        {
            var data = File.ReadAllText(inputFile);
            return data;
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
                                case "smallimage":
                                    e.Smallimage = value;
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
                                            var yearPos = claimAndText.IndexOf(e.Year);
                                            if (yearPos > 0)
                                            {
                                                var headline = claimAndText.Substring(0, yearPos).Trim();
                                                headline = headline.TrimEnd('-');
                                                headline = headline.Trim();
                                                e.Headline = headline;
                                                var facebook = claimAndText.Substring(yearPos).Trim();
                                                //var likePos = facebook.IndexOf("(");
                                                //if (likePos > 0)
                                                //{
                                                //    facebook = facebook.Substring(0, likePos).Trim();
                                                //}
                                                e.Facebook = facebook;
                                            }
                                        }
                                        else
                                        {
                                            e.Facebook = claimAndText;
                                        }
                                        if (fbParts.Length == 3)
                                        {
                                            e.Facebook2 = fbParts[1].Trim();
                                            e.Facebook3 = fbParts[2].Trim();
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
                                case "author":
                                    e.Author = value;
                                    break;
                                case "translation":
                                    e.Translation = value;
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

                {
                    var dot = e.Image.IndexOf('.');
                    if (dot > 0)
                    {
                        e.Name = e.Image.Substring(0, dot);
                    }
                }

                e.Post = e.Facebook;
                if (!string.IsNullOrEmpty(e.Facebookimage))
                {
                    e.Postimage = e.Facebookimage;
                }
                if (!string.IsNullOrEmpty(e.Twitterimage))
                {
                    e.Postimage = e.Twitterimage; // some years ago larger than FB image, later the same
                }

                timeline.Add(e);
                lineCount++;
            }

            return timeline;
        }

        public string CreateYamlData(TimelineEntry e)
        {
            var s = "";
            if (!string.IsNullOrEmpty(e.Name)) { s += $"Name: {e.Name}\r\n"; }
            s += $"Year: {YamlValue(e.Year)}\r\n";
            s += $"Title: {YamlValue(e.Title)}\r\n";
            if (!string.IsNullOrEmpty(e.Short)) { s += $"Short: {YamlValue(e.Short)}\r\n"; }
            if (!string.IsNullOrEmpty(e.Summary)) { s += $"Summary: {YamlValue(e.Summary)}\r\n"; }
            if (!string.IsNullOrEmpty(e.Image)) { s += $"Image: {YamlValue(e.Image)}\r\n"; }
            if (!string.IsNullOrEmpty(e.Headline)) { s += $"Headline: {YamlValue(e.Headline)}\r\n"; }
            if (!string.IsNullOrEmpty(e.Smallimage)) { s += $"SmallImage: {YamlValue(e.Smallimage)}\r\n"; }
            if (!string.IsNullOrEmpty(e.Post)) { s += $"Post: {YamlValue(e.Post)}\r\n"; }
            if (!string.IsNullOrEmpty(e.Postimage)) { s += $"PostImage: {YamlValue(e.Postimage)}\r\n"; }
            if (!string.IsNullOrEmpty(e.Twitter)) { s += $"Twitter: {YamlValue(e.Twitter)}\r\n"; }
            if (!string.IsNullOrEmpty(e.Twitterimage)) { s += $"TwitterImage: {YamlValue(e.Twitterimage)}\r\n"; }
            if (!string.IsNullOrEmpty(e.Facebook)) { s += $"Facebook: {YamlValue(e.Facebook)}\r\n"; }
            if (!string.IsNullOrEmpty(e.Facebook2)) { s += $"Facebook2: {YamlValue(e.Facebook2)}\r\n"; }
            if (!string.IsNullOrEmpty(e.Facebook3)) { s += $"Facebook3: {YamlValue(e.Facebook3)}\r\n"; }
            if (!string.IsNullOrEmpty(e.Facebookimage)) { s += $"FacebookImage: {YamlValue(e.Facebookimage)}\r\n"; }
            if (!string.IsNullOrEmpty(e.Author)) { s += $"Author: {YamlValue(e.Author)}\r\n"; }
            if (!string.IsNullOrEmpty(e.Translation)) { s += $"Translation: {YamlValue(e.Translation)}\r\n"; }
            if (e.Tags.Count > 0) { s += $"Tags: [{string.Join(", ", e.Tags.Select(x => YamlValue(x)))}]\r\n"; }
            if (e.Topics.Count > 0) { s += $"Topics: [{string.Join(", ", e.Topics.Select(x => YamlValue(x)))}]\r\n"; }
            if (e.Text.Count > 0) { s += $"Text: |\r\n{string.Join("\r\n", e.Text.Select(x => "  " + x))}\r\n"; }
            return s;
        }

        public void Write(List<TimelineEntry> entries, string outputFolder, string imgFolder, string snImgFolder)
        {
            Directory.CreateDirectory(Path.Combine(outputFolder, YamlImageFolderName));

            var config = $@"images: ./{YamlImageFolderName}/
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
";
            var years = new Dictionary<string, int>();
            foreach (var e in entries)
            {
                var outputFilePath = GetFilePath(e, outputFolder, years);

                if (!string.IsNullOrEmpty(e.Image))
                {
                    var src = Path.Combine(imgFolder, e.Image);
                    var dst = Path.Combine(outputFolder, YamlImageFolderName, e.Image);
                    if (File.Exists(src))
                    {
                        File.Copy(src, dst, overwrite: true);
                    }
                    else
                    {
                        e.Image = "";
                    }
                }

                if (!string.IsNullOrEmpty(e.Smallimage))
                {
                    var src = Path.Combine(imgFolder, e.Smallimage);
                    var dst = Path.Combine(outputFolder, YamlImageFolderName, e.Smallimage);
                    if (File.Exists(src))
                    {
                        File.Copy(src, dst, overwrite: true);
                    }
                    else
                    {
                        e.Smallimage = "";
                    }
                }

                if (!string.IsNullOrEmpty(e.Facebookimage))
                {
                    var src = Path.Combine(snImgFolder, e.Facebookimage);
                    var dst = Path.Combine(outputFolder, YamlImageFolderName, e.Facebookimage);
                    if (File.Exists(src))
                    {
                        File.Copy(src, dst, overwrite: true);
                    }
                    else
                    {
                        e.Facebookimage = "";
                    }
                }

                if (!string.IsNullOrEmpty(e.Twitterimage))
                {
                    var src = Path.Combine(snImgFolder, e.Twitterimage);
                    var dst = Path.Combine(outputFolder, YamlImageFolderName, e.Twitterimage);
                    if (File.Exists(src))
                    {
                        File.Copy(src, dst, overwrite: true);
                    }
                    else
                    {
                        e.Twitterimage = "";
                    }
                }

                var entry = CreateYamlData(e);
                Log.Info(Path.GetFileName(outputFilePath));

                File.WriteAllText(outputFilePath, entry);
            }

            File.WriteAllText(Path.Combine(outputFolder, "config.yaml"), config);
        }

        public string GetFilePath(TimelineEntry e, string outputFolder, Dictionary<string, int> years)
        {
            var file = "";
            
            if (string.IsNullOrEmpty(file))
            {
                file = e.Image;
                if (!string.IsNullOrEmpty(file))
                {
                    var dot = file.IndexOf('.');
                    if (dot > 2)
                    {
                        file = file.Substring(0, dot);
                    }
                }
            }
        
            if (string.IsNullOrEmpty(file))
            {
                file = e.Title
                .Replace(" ", "_")
                .Replace("/", "")
                .Replace(":", "")
                .Replace("-", "")
                .Replace(",", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("\"", "")
                ;
                var dot = file.IndexOf('.');
                if (dot > 0)
                {
                    file = file.Substring(0, dot);
                }
                file = file.Truncate(30, "");
            }

            if (!years.ContainsKey(e.Year))
            {
                years[e.Year] = 0;
            }
            else
            {
                years[e.Year] += 1;
            }

            var suffix = "acfilortux";
            file = e.Year + (years[e.Year] == 0 ? "" : "" + suffix[years[e.Year]]) + "_" + file;

            return Path.Combine(outputFolder, file + ".yaml");
        }

        public string YamlValue(string value)
        {
            if (value.Contains(": ") || value.Contains(" #"))
            {
                return WrapValue(value);
            }
            return value;
        }

        public string WrapValue(string value)
        {
            var quote = "\"";
            if (value.Contains(quote))
            {
                quote = "'";
            }
            if (value.Contains(quote))
            {
                quote = "\"";
            }
            return quote + value.Replace(quote, "\\" + quote) + quote;
        }
    }
}
