using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using GaldevWeb;

namespace n3q.GaldevWeb.Controllers
{
    [ApiController]
    public class CultureController : GaldevControllerBase
    {
        public CultureController(GaldevApp app) : base(app) { }

        [Route("[controller]")]
        [HttpGet]
        public ActionResult SetCulture(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            return Redirect(Request.Headers["Referer"].ToString());
        }

    }
}