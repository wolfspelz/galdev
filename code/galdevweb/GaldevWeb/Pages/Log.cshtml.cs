using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using GaldevWeb;
using System.Text.RegularExpressions;

namespace GaldevWeb.Pages
{
    public class LogModel : GaldevPageModel
    {
        public bool Colored = false;
        public List<string> Lines = new List<string>();
        InMemoryLoggerProvider _loggerProvider;

        public LogModel(GaldevApp app, InMemoryLoggerProvider loggerProvider) : base(app, "Log")
        {
            _loggerProvider = loggerProvider;
        }

        public void OnGet(int lines = 0, bool colored = false, bool filtered = true)
        {
            Colored = colored;

            int numLines = Config.MaxLogLines;
            if (lines > 0) {
                numLines = lines;
            }

            Lines = _loggerProvider
                .GetLogs()
                .Where(line => !filtered || LineFilter(line))
                .Take(numLines)
                .ToList();
        }

        private bool LineFilter(string line)
        {
            // Check line against regex based deny list in Config.LogLineRegexDenyList
            foreach (var regex in Config.LogLineRegexDenyList) {
                if (Regex.IsMatch(line, regex)) {
                    return false;
                }
            }
            return true;
        }
    }
}