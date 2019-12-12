using System;
using System.Linq;

namespace galdevtool
{
    public class Program
    {
        public AppConfig Config { get; set; }
        //public ICallbackLogger Log { get; set; } = new NullCallbackLogger();
        public ITimeSource TimeSource { get; set; } = new UtcTimeSource();

        static void Main(string[] args)
        {
            var program = new Program();
            program.Run();

            Console.ReadKey();
        }

        private void Run()
        {
            Config = new AppConfig().Initialize();
            Log.LogFile = Config.LogFile;
            Log.LogLevel = Log.LevelFromString(Config.LogLevels);
            Log.LogHandler = (level, context, message) => { Console.WriteLine($"{Log.LevelFromString(level.ToString())} {context} {message}"); };
            Log.Info("-----------------------------------------------");
            Log.Info(string.Join(" ", Environment.GetCommandLineArgs().Select(x => "[" + x + "]")));
            Config.ParseCommandline(Environment.GetCommandLineArgs().ToList());
            Log.LogFile = Config.LogFile;
            Log.LogLevel = Log.LevelFromString(Config.LogLevels);

            if (Config.ShowHelp)
            {
                ShowHelp();
            }
            else
            {
                try
                {
                    if (false) { } else 
                    if (Config.Bigfile2Yaml)
                    {
                        new Bigfile2Yaml()
                        { Log = new GlobalCallbackLogger(nameof(Bigfile2Yaml)), Config = this.Config }
                        .Convert();
                    }
                    if (Config.Yaml2Bigfile)
                    {
                        new Yaml2Bigfile()
                        { Log = new GlobalCallbackLogger(nameof(Yaml2Bigfile)), Config = this.Config }
                        .Convert();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }

        private void ShowHelp()
        {
            Log.Info("Commandline arguments:");
            Log.Info("Help | Show commandline arguments");
            Log.Info("Debug | Run in debug mode");
            Log.Info("<name>=<value> | Set Config option:");
            foreach (var key in Config
                .GetAll()
                .Keys
                .Except(AppConfig.HiddenKeys)
                .OrderBy(s => s)
            )
            {
                Log.Info($"  {key}={Config.GetAsString(key)}");
            }
        }
    }
}
