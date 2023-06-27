using Microsoft.AspNetCore.HttpOverrides;
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

            var config = new GaldevConfig();
            builder.Services.AddSingleton(new GaldevApp { Config = config });

            var timeline = new TimelineIndex { IndexFilePath = config.IndexPath, };
            timeline.Init();
            builder.Services.AddSingleton(timeline);

            var app = builder.Build();

            if (!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRequestLocalization(new[] { "en-US", "de-DE" }); // Sets Thread.CurrentThread.CurrentUICulture

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
