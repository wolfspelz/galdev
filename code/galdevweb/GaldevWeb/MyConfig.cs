using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaldevWeb
{
    public class MyConfig : MemoryCallbackConfig
    {
        public string AppName = "GaldevWeb";
        public string IndexPath = "../../../data/index.yaml";
    }
}
