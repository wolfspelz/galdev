using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;

namespace GaldevWeb
{
    public static class Program
    {
        public static void Main(string[] args)
        {
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
                options.SetDefaultCulture(supportedCultures[0])
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures);
            });

            var myConfig = new GaldevConfig();
            var myApp = new GaldevApp { Config = myConfig };
            builder.Services.AddSingleton(myApp);

            var timeline = new TimelineIndex {
                IndexFilePath = myConfig.IndexPath,
                Log = myApp.Log,
            };
            timeline.Load();
            builder.Services.AddSingleton(timeline);

            BlogIndex? blog = new BlogIndex {
                IndexFilePath = myConfig.BlogIndexPath,
                Log = myApp.Log,
            };
            blog.Load();
            builder.Services.AddSingleton(blog);

            var app = builder.Build();

            if (!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

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

            app.UseForwardedHeaders(new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.Run();
        }
    }

    public class Galdev { }
}
