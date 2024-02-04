using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;

namespace GaldevWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var cwd = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            //if (!string.IsNullOrEmpty(cwd)) {
            //    Directory.SetCurrentDirectory(cwd);
            //}

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
            builder.Services.AddControllers();
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

            builder.Services.AddLocalization();

            builder.Services.AddMvc()
              .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
              .AddDataAnnotationsLocalization();

            builder.Services.Configure<RequestLocalizationOptions>(options => {
                var supportedCultures = new[] { "en-US", "de-DE" };
                options.SetDefaultCulture("de-DE")
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures);
            });

            var myConfig = new GaldevConfig();
            var myLogger = new NullCallbackLogger();

            var timeIndex = new TimelineIndex {
                IndexFilePath = myConfig.DataIndexPath,
                Log = myLogger,
            };
            timeIndex.Load(entry => !entry.Tags.Contains("_hidden") && !entry.Tags.Contains("_noweb"));

            Don.t = () => {
                BlogIndex? blog = new BlogIndex {
                    IndexFilePath = myConfig.BlogIndexPath,
                    Log = myLogger,
                };
                blog.Load();
                builder.Services.AddSingleton(blog);
            };

            var myApp = new GaldevApp {
                Config = myConfig,
                Log = myLogger,
                Timelines = timeIndex,
            };
            builder.Services.AddSingleton(myApp);

            var app = builder.Build();

            app.UseForwardedHeaders(new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            //if (!app.Environment.IsDevelopment()) {
            //    app.UseExceptionHandler("/Error");
            //}
            app.UseStaticFiles(new StaticFileOptions() {
                ServeUnknownFileTypes = true
            });

            var supportedCultures = new[]{
                new CultureInfo("de-DE"),
                new CultureInfo("en-US"),
            };
            var localizationOptions = new RequestLocalizationOptions {
                DefaultRequestCulture = new RequestCulture("de-DE"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
            };
            localizationOptions.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider { QueryStringKey = "lang" });
            localizationOptions.RequestCultureProviders.Insert(1, new CookieRequestCultureProvider());
            localizationOptions.RequestCultureProviders.Insert(2, new AcceptLanguageHeaderRequestCultureProvider());
            app.UseRequestLocalization(localizationOptions); // Sets Thread.CurrentThread.CurrentUICulture

            app.UseCors();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

            app.Run();
        }
    }

    public class Galdev { }
}
