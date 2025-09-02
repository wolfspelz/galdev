using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;

public class ImageToWebpConversionMiddlewareOptions
{
    public int CacheDurationSec { get; set; } = 3600; // Default cache duration set to 1 hour
}

public class ImageToWebpConversionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;
    private readonly ImageToWebpConversionMiddlewareOptions _options;

    public ImageToWebpConversionMiddleware(RequestDelegate next, IMemoryCache cache, IOptions<ImageToWebpConversionMiddlewareOptions> options)
    {
        _next = next;
        _cache = cache;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBody = context.Response.Body;

        try {
            using var memStream = new MemoryStream();
            context.Response.Body = memStream;

            await _next(context);

            if (ShouldConvertToWebp(context)) {
                await ConvertToWebpAndSendResponse(context, memStream, originalBody);
            } else {
                memStream.Seek(0, SeekOrigin.Begin);
                await memStream.CopyToAsync(originalBody);
            }
        } finally {
            context.Response.Body = originalBody;
        }
    }

    private bool ShouldConvertToWebp(HttpContext context)
    {
        return context.Response.ContentType != null &&
               (context.Response.ContentType.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase) ||
                context.Response.ContentType.Equals("image/png", StringComparison.OrdinalIgnoreCase));
    }

    private async Task ConvertToWebpAndSendResponse(HttpContext context, MemoryStream memStream, Stream originalBody)
    {
        var cacheKey = $"WEBP_{context.Request.Path}_{memStream.Length}";

        if (!_cache.TryGetValue(cacheKey, out byte[]? webpImage)) {
            memStream.Seek(0, SeekOrigin.Begin);
            using var image = SixLabors.ImageSharp.Image.Load<Rgba32>(memStream);
            using var webpStream = new MemoryStream();
            await image.SaveAsync(webpStream, new WebpEncoder());
            webpImage = webpStream.ToArray();

            var cacheOptions = new MemoryCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_options.CacheDurationSec)
            };
            _cache.Set(cacheKey, webpImage, cacheOptions);
        }

        if (webpImage == null) {
            throw new InvalidOperationException("WebP conversion failed.");
        }
        context.Response.ContentType = "image/webp";
        context.Response.ContentLength = webpImage.Length;
        await originalBody.WriteAsync(webpImage, 0, webpImage.Length);
    }
}