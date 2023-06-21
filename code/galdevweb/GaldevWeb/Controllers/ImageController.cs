using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

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

            var filePath = new Timeline { IndexFilePath = Config.IndexPath, }.GetImagePath(name, lang);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out string? contentType)) {
                contentType = "application/octet-stream";
            }

            var stream = System.IO.File.OpenRead(filePath);
            return new FileStreamResult(stream, contentType);
        }
    }
}
