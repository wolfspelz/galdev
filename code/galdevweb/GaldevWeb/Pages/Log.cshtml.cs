using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaldevWeb.Pages
{
    public class LogModel : GaldevPageModel
    {
        public LogModel(GaldevApp app) : base(app, "Log")
        {
        }

        public void OnGet()
        {
            Log.Info($"");
        }
    }
}