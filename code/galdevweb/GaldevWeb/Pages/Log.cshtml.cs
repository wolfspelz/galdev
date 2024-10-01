using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaldevWeb.Pages
{
    public class LogModel : GaldevPageModel
    {
        public int Lines = 0;

        public LogModel(GaldevApp app) : base(app, "Log")
        {
        }

        public void OnGet(int lines = 0)
        {
            //Log.Info($"");
            if (lines > 0) {
                Lines = lines;
            } else {
                Lines = Config.MaxLogLines;
            }
        }
    }
}