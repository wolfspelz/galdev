using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace galdevtool
{
    public class AppConfig : ConfigBase, ICallbackConfig
    {
        public enum RunMode
        {
            Development,
            Test,
            Staging,
            Production
        }

        public RunMode Mode { get; set; } = RunMode.Production;
        public bool IsInitialized { get; set; } = false;

        public bool ShowHelp { get; set; } = false;
        public bool Bigfile2Yaml { get; set; } = false;
        public bool Yaml2Bigfile { get; set; } = false;

        public string LogLevels { get; set; } = "Error,Warning";
        public string LogFile { get; set; } = "%TEMP%galdevtool.log";

        public int TestInt { get; set; } = 42;

        public string Bigfile2YamlFilePath { get; set; } = @"C:\Heiner\github-galdev\code\galdevtool\tmp\orig\data2.txt";
        public string Bigfile2YamlImagePath { get; set; } = @"C:\Heiner\wolfspelz-www.galactic-developments.de\images";
        public string Bigfile2YamlSnImagePath { get; set; } = @"C:\Heiner\wolfspelz-www.galactic-developments.de\images\post";
        public string Bigfile2YamlYamlFolderPath { get; set; } = @"C:\Heiner\github-galdev\code\galdevtool\tmp\yaml";

        public string Yaml2BigfileYamlFolderPath { get; set; } = @"C:\Heiner\github-galdev\code\galdevtool\tmp\yaml";
        public string Yaml2BigfileFilePath { get; set; } = @"C:\Heiner\github-galdev\code\galdevtool\tmp\back";
        public string Yaml2BigfileImagePath { get; set; } = @"C:\Heiner\wolfspelz-www.galactic-developments.de\images";
        public string Yaml2BigfileSnImagePath { get; set; } = @"C:\Heiner\wolfspelz-www.galactic-developments.de\images\post";

        public static string[] HiddenKeys { get; set; } = {
            nameof(AppConfig.TestInt),
        };

        public AppConfig Initialize()
        {
            if (IsInitialized)
            {
                return this;
            }
            IsInitialized = true;

            // ReSharper disable once RedundantAssignment
            bool isDebugBuild = false;
#if DEBUG
            isDebugBuild = true;
#endif
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            Mode = isDebugBuild ? RunMode.Development : RunMode.Production;

            if (Environment.GetCommandLineArgs().Intersect(new List<string> { "--dev", "--debug", "Debug" }).Any())
            {
                Mode = RunMode.Development;
            }
            if (Environment.GetCommandLineArgs().Intersect(new List<string> { "--prod", "--production", "--release", "Release" }).Any())
            {
                Mode = RunMode.Production;
            }

            switch (Mode)
            {
                case RunMode.Development:
                    //LogLevels = "Error,Warning,Debug,User,Info";
                    LogLevels = "Error,Warning,Debug,User,Info,Verbose";
                    break;

                case RunMode.Production:
                    LogLevels = "Error,Warning,Debug,User,Info";
                    break;
            }

            return this;
        }

        public void ParseCommandline(IList<string> args)
        {
            BeforeCommandline();

            var q = new Queue<string>(args);
            while (q.Count > 0)
            {
                var arg = q.Dequeue();
                arg = arg.Trim();
                switch (arg)
                {
                    case "-?":
                    case "/?":
                    case "--help":
                    case "help":
                    case "Help":
                        ShowHelp = true;
                        break;
                    case "Bigfile2Yaml":
                        Bigfile2Yaml = true;
                        break;
                    case "Yaml2Bigfile":
                        Yaml2Bigfile = true;
                        break;
                    default:
                        var kv = arg.Split(new[] { '=' }, 2);
                        if (kv.Length == 2)
                        {
                            try
                            {
                                if (Set(kv[0], kv[1]))
                                {
                                    Log.Info($"{kv[0]}={kv[1]}");
                                }
                                else
                                {
                                    Log.Warning($"No such option: {arg}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Warning(ex);
                            }
                        }
                        break;
                }
            }

            AfterCommandline();
        }

        private void BeforeCommandline()
        {
        }

        public void AfterCommandline()
        {
        }

        object ICallbackConfig.Get(string name, object defaultValue)
        {
            var value = Get(name);
            if (value == null)
            {
                Log.Warning($"No config for: {name}");
            }

            return value ?? defaultValue;
        }

        void ICallbackConfig.Set(string name, object value)
        {
            string s;
            if (value is string alreadyString)
            {
                s = alreadyString;
            }
            else if (value is float f)
            {
                s = f.ToString(CultureInfo.InvariantCulture);
            }
            else if (value is double d)
            {
                s = d.ToString(CultureInfo.InvariantCulture);
            }
            else if (value is int i)
            {
                s = i.ToString(CultureInfo.InvariantCulture);
            }
            else if (value is long l)
            {
                s = l.ToString(CultureInfo.InvariantCulture);
            }
            else if (value is DateTime dt)
            {
                s = dt.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                s = value.ToString();
            }

            Set(name, s);
        }
    }
}
