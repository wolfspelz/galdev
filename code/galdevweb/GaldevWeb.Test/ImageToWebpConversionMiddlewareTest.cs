using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SkiaSharp;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GaldevWeb.Test;

[TestClass]
public class ImageToWebpConversionMiddlewareTest
{
    static byte[] MakePng(int width, int height)
    {
        using var bitmap = new SKBitmap(width, height);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.CornflowerBlue);
        using var data = bitmap.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }

    static async Task<(HttpContext context, MemoryStream body)> Run(string contentType, byte[] payload, string path = "/image.png")
    {
        var body = new MemoryStream();
        var context = new DefaultHttpContext();
        context.Request.Path = path;
        context.Response.Body = body;

        var middleware = new ImageToWebpConversionMiddleware(
            next: async ctx => {
                ctx.Response.ContentType = contentType;
                await ctx.Response.Body.WriteAsync(payload);
            },
            cache: new MemoryCache(new MemoryCacheOptions()),
            options: Options.Create(new ImageToWebpConversionMiddlewareOptions())
        );

        await middleware.InvokeAsync(context);
        return (context, body);
    }

    [TestMethod]
    [TestCategory("GaldevWeb.Middleware")]
    public async Task Png_is_converted_to_webp()
    {
        // Arrange
        var png = MakePng(64, 32);

        // Act
        var (context, body) = await Run("image/png", png);
        var bytes = body.ToArray();

        // Assert
        Assert.AreEqual("image/webp", context.Response.ContentType);
        Assert.AreEqual(bytes.Length, context.Response.ContentLength);
        Assert.AreEqual("RIFF", Encoding.ASCII.GetString(bytes, 0, 4));
        Assert.AreEqual("WEBP", Encoding.ASCII.GetString(bytes, 8, 4));
        using var decoded = SKBitmap.Decode(bytes);
        Assert.IsNotNull(decoded);
        Assert.AreEqual(64, decoded.Width);
        Assert.AreEqual(32, decoded.Height);
    }

    [TestMethod]
    [TestCategory("GaldevWeb.Middleware")]
    public async Task Non_image_is_passed_through_unchanged()
    {
        // Arrange
        var html = Encoding.UTF8.GetBytes("<html><body>hello</body></html>");

        // Act
        var (context, body) = await Run("text/html", html, "/index.html");

        // Assert
        Assert.AreEqual("text/html", context.Response.ContentType);
        CollectionAssert.AreEqual(html, body.ToArray());
    }

    [TestMethod]
    [TestCategory("GaldevWeb.Middleware")]
    public async Task Undecodable_image_is_passed_through_unchanged()
    {
        // Arrange
        var garbage = Encoding.ASCII.GetBytes("this is not a png");

        // Act
        var (context, body) = await Run("image/png", garbage, "/broken.png");

        // Assert
        Assert.AreEqual("image/png", context.Response.ContentType);
        CollectionAssert.AreEqual(garbage, body.ToArray());
    }
}
