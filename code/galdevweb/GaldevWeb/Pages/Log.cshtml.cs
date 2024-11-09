using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaldevWeb.Pages
{
    public class LogModel : GaldevPageModel
    {
        public int NumLines = 0;
        public bool Colored = false;

        public LogModel(GaldevApp app) : base(app, "Log")
        {
        }

        public void OnGet(int lines = 0, bool colored = false)
        {
            Colored = colored;

            if (lines > 0) {
                NumLines = lines;
            } else {
                NumLines = Config.MaxLogLines;
            }
        }
    }
}