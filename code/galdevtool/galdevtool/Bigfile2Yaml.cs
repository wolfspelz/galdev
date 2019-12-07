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

        public void Write(List<TimelineEntry> entries, string outputFolder)
        {
            var index = @"images: ./images/
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

                var entry = $@"year: {e.Year}
title: {e.Title}
short: {e.Short}
summary: {e.Summary}
headline: {e.Headline}
image: {e.Image}
smallimage: {e.Smallimage}
tags: 
{string.Join("\r\n", e.Tags.Select(x => "  - " + x))}
twitter: {e.Twitter}
twitterimage: {e.Twitterimage}
facebook: {e.Facebook}
facebookimage: {e.Facebookimage}
topics: 
{string.Join("\r\n", e.Topics.Select(x => "  - " + x))}
text: |
{string.Join("\r\n", e.Text.Select(x => "  " + x))}
";

                File.WriteAllText(Path.Combine(outputFolder, name + ".yaml"), entry);
            }

            File.WriteAllText(Path.Combine(outputFolder, "index.yaml"), index);
        }

    }
}
