using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace galdevtool
{
    public class AppConfig : ConfigBase
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
        public bool CountCharacters { get; set; } = false;

        public string LogLevels { get; set; } = "Error,Warning";
        public string LogFile { get; set; } = "%TEMP%galdevtool.log";

        public bool WaitOnException { get; set; } = false;
        public bool WaitOnFinished { get; set; } = false;

        public int TestInt { get; set; } = 42;

        public string Bigfile2YamlInputYamlFilePath { get; set; } = "";
        public string Bigfile2YamlInputImageFolderPath { get; set; } = "";
        public string Bigfile2YamlInputSnImagePath { get; set; } = "";
        public string Bigfile2YamlOutputFolderPath { get; set; } = "";

        public string Yaml2BigfileInputFolderPath { get; set; } = "";
        public string Yaml2BigfileOutputFilePath { get; set; } = "";
        public string Yaml2BigfileOutputImagePath { get; set; } = "";
        public string Yaml2BigfileOutputSnImagePath { get; set; } = "";

        public string YamlImageFolderName { get; set; } = @"images";

        public string CountCharactersDataFolderPath { get; set; } = "";

        public static string[] HiddenKeys { get; set; } = {
            nameof(AppConfig.TestInt),
        };

        public AppConfig Initialize()
        {
            if (IsInitialized) {
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

            if (Environment.GetCommandLineArgs().Intersect(new List<string> { "--dev", "--debug", "Debug" }).Any()) {
                Mode = RunMode.Development;
            }
            if (Environment.GetCommandLineArgs().Intersect(new List<string> { "--prod", "--production", "--release", "Release" }).Any()) {
                Mode = RunMode.Production;
            }

            switch (Mode) {
                case RunMode.Development:
                    //LogLevels = "Error,Warning,Debug,User,Info";
                    LogLevels = "Error,Warning,Debug,User,Info,Verbose";
                    WaitOnException = true;
                    break;

                case RunMode.Production:
                    LogLevels = "Error,Warning";
                    break;
            }

            return this;
        }

        protected override void HandleCommandlineParameter(string arg)
        {
            switch (arg) {
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
                case "CountCharacters":
                    CountCharacters = true;
                    break;
                default:
                    var kv = arg.Split(new[] { '=' }, 2);
                    if (kv.Length == 2) {
                        try {
                            if (Set(kv[0], kv[1])) {
                                Log.Info($"{kv[0]}={kv[1]}");
                            } else {
                                Log.Warning($"No such option: {arg}");
                            }
                        } catch (Exception ex) {
                            Log.Warning(ex);
                        }
                    }
                    break;
            }
        }

    }
}
