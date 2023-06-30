namespace GaldevWeb.Pages
{
    public class PrivacyModel : GaldevPageModel
    {
        public PrivacyModel(GaldevApp app) : base(app, "Privacy")
        {
        }

        public void OnGet()
        {
            Log.Info("", new LogData { });
        }
    }
}