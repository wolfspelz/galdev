using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
using n3q.FrameworkTools;

namespace GaldevWeb;

public class Program
{
    public static void Main(string[] args)
    {
        //var cwd = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
        //if (!string.IsNullOrEmpty(cwd)) {
        //    Directory.SetCurrentDirectory(cwd);
        //}
        var myConfig = new GaldevConfig();

        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ClearProviders();
        builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();
        var inMemoryLoggerProvider = new InMemoryLoggerProvider(myConfig.MaxLogLines);
        builder.Logging.AddProvider(inMemoryLoggerProvider);
        builder.Logging.AddFilter("Microsoft.AspNetCore.Watch.BrowserRefresh.BrowserRefreshMiddleware", LogLevel.None);
        builder.Logging.AddFilter("Microsoft.WebTools.BrowserLink.Net.BrowserLinkMiddleware", LogLevel.None);

        var razorPagesBuilder = builder.Services.AddRazorPages();
        var mvcBuilder = builder.Services.AddControllersWithViews();
        
        if (builder.Environment.IsDevelopment())
        {
            razorPagesBuilder.AddRazorRuntimeCompilation();
            mvcBuilder.AddRazorRuntimeCompilation();
        }
        
        builder.Services.AddControllers();

        builder.Services.AddLocalization();

        builder.Services.AddMvc()
          .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
          .AddDataAnnotationsLocalization();

        //builder.Services.AddMemoryCache();

        builder.Services.Configure<ImageToWebpConversionMiddlewareOptions>(options =>
        {
            options.CacheDurationSec = myConfig.WebpMemoryCacheDurationSec;
        });

        builder.Services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.MimeTypes = new[]
            {
                "text/html",
                "text/css",
                "application/javascript",
                "text/javascript",
                "application/json",
                "application/xml",
                "text/plain",
                "image/svg+xml"
            };
        });

        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[] { "en-US", "de-DE" };
            options.SetDefaultCulture("de-DE")
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
        });

        //BlogIndex? blog = new BlogIndex {
        //    IndexFilePath = myConfig.BlogIndexPath,
        //    Log = myLogger,
        //};
        //blog.Load();
        //builder.Services.AddSingleton(blog);

        var timeIndex = new TimelineIndex
        {
            IndexFilePath = myConfig.DataIndexPath,
        };

        var myApp = new GaldevApp
        {
            Config = myConfig,
            Timelines = timeIndex,
        };
        builder.Services.AddSingleton(myApp);

        builder.Services.AddSingleton(inMemoryLoggerProvider);

        var app = builder.Build();

        var myLogger = new MicrosoftLoggingCallbackLogger(app.Services.GetService<ILogger<Galdev>>());
        timeIndex.Log = myLogger;
        myApp.Log = myLogger;

        timeIndex.Load(entry => !entry.Tags.Contains("_hidden") && !entry.Tags.Contains("_noweb"));

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
            KnownNetworks = { },
            KnownProxies = { },
        });

        //if (!app.Environment.IsDevelopment()) {
        //    app.UseExceptionHandler("/Error");
        //}

        //app.UseMiddleware<ImageToWebpConversionMiddleware>();
        app.UseStaticFiles(new StaticFileOptions()
        {
            ServeUnknownFileTypes = true
        });

        app.UseResponseCompression();
        app.UseRouting();

        var supportedCultures = new[]{
            new CultureInfo("de-DE"),
            new CultureInfo("en-US"),
        };
        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("de-DE"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures,
        };
        localizationOptions.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider { QueryStringKey = "lang" });
        localizationOptions.RequestCultureProviders.Insert(1, new CookieRequestCultureProvider());
        localizationOptions.RequestCultureProviders.Insert(2, new AcceptLanguageHeaderRequestCultureProvider());
        app.UseRequestLocalization(localizationOptions); // Sets Thread.CurrentThread.CurrentUICulture

        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllers();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.Run();
    }
}

public class Galdev { }
