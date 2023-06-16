using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace GaldevWeb.Pages
{
    public class TimelineEntry
    {
        public string Year;
        public string Title;
        public string[] Text;
        public string? Summary;
        public string? Image;

        public TimelineEntry(string year, string title, string[] text)
        {
            Year = year;
            Title = title;
            Text = text;
        }
    }

    public class IndexModel : AppPageModel
    {
        public TimelineEntry? Entry;

        public IndexModel(MyApp app) : base(app, "Index") { }

        public void OnGet(string name)
        {
            if (Is.Value(name)) {

                string id = GetIdFromFriendlyName(name);
                var indexPath = Config.IndexPath; // Path.GetDirectoryName(
                var indexNode = JsonPath.Node.FromJson(@"
{
   'languages': {
        'de': {
            'path': './de',
            'images': './de/images',
            'topics': {
                'accident': './Unfälle und Havarien',
                'path': './Abenteuer',
            }
        }
    },
   'translations': {
       'NeuroImplant': {
            'de': '2051_Neuroimplantat',
            'en': '2051_NeuroImplant',
        }
    }
}", new DeserializerOptions { LowerCaseKeys = true });
                var lang = GetLangFromCultureName(UiCultureName);
                var langPath = indexNode["languages"][lang]["path"].AsString;
                var entryFileName = indexNode["translations"][id][lang].AsString;
                var dirPath = Path.GetDirectoryName(Config.IndexPath);
                var entryPath = Path.Combine(dirPath ?? "", langPath, entryFileName);
                var entryNode = JsonPath.Node.FromJson(@"{
    'Year': '2051',
    'Title': 'Mikromechanisches Neuroimplantat. Chengdu/China',
    'Short': 'Das einzige unbenutzte Modell der Serie-H eines mikromechanischen AV-Neuroimplantats vom Chengdu Zentrum für Medizintechnologie...',
    'Text': 'Das einzige erhaltene Modell des Sehnervadapters der Serie-H eines mikromechanischen AV-Neuroimplantats vom Chengdu Zentrum für Medizintechnologie.\r\nDas Implantat speist künstliche Signale in Seh- und Hörnerven ein. Es stammt aus der 22. klinischen Versuchsreihe, die vor allem zum Ziel hatte, blinden und tauben Menschen zu helfen. Das Gerät besteht aus fünf Komponenten, je zwei für Seh- und Hörnerven sowie ein zentrales Steuerungsmodul. Die Elemente sind jeweils etwa fünf Millimeter groß. Sie werden in einer Operation weitgehend minimalinvasiv in den Zielregionen im Kopf platziert.',
}", new DeserializerOptions { LowerCaseKeys = true });
                var year = entryNode["year"].AsString;
                var title = entryNode["title"].AsString;
                var text = entryNode["text"].AsString
                    .Replace("\r\n", "\n")
                    .Replace("\r", "\n")
                    .Split('\n')
                    .Select(x => x.Trim())
                    .ToArray();

                Entry = new TimelineEntry(year, title, text);

                var summary = entryNode["summary"].AsString;
                if (Is.Value(summary)) {
                    Entry.Summary = summary;
                }

                var shortSummary = entryNode["short"].AsString;
                if (Is.Value(shortSummary)) {
                    Entry.Summary = shortSummary;
                }

            } else {
            }
        }

        protected static string GetIdFromFriendlyName(string name)
        {
            var parts = name.ToLower().Split(new char[] { '-', ':' }, 2);
            return parts[0];
        }

        protected static string GetLangFromCultureName(string cultureName)
        {
            return cultureName.Split(new char[] { '-', '_' }).First().ToLower();
        }
    }
}