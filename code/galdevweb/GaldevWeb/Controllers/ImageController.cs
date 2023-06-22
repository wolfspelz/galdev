using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;

namespace GaldevWeb.Controllers
{
    public class ImageController : AppControllerBase
    {
        public ImageController(MyApp app) : base(app)
        {
        }

        [HttpGet]
        [Route("[controller]/{lang}/{name}")]
        public IActionResult Get(string lang, string name)
        {
            Log.Info("", new LogData { [nameof(lang)] = lang, [nameof(name)] = name });

            var filePath = new I18nTimeline { IndexFilePath = Config.IndexPath, }.GetImagePath(name, lang);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out string? contentType)) {
                contentType = "application/octet-stream";
            }

            if (!System.IO.File.Exists(filePath)) {
                filePath = Config.NotFoundImagePath;
            }

            var stream = System.IO.File.OpenRead(filePath);
            return new FileStreamResult(stream, contentType);
        }
    }
}
