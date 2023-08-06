using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Threading.Tasks;

namespace GaldevWeb.Test
{
    [TestClass]
    public class PagesTest
    {
        HttpClient _client = new();

        [TestInitialize]
        public void Startup()
        {
            _client = new WebApplicationFactory<GaldevWeb.Program>()
                .WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot("code/galdevweb/GaldevWeb"))
                .CreateClient();
        }

        [TestMethod]
        [TestCategory("GaldevWeb.Pages")]
        public async Task Index()
        {
            // Act
            var response = await _client.GetAsync("/Index?lang=en_US");

            // Assert
            Assert.IsNotNull(response.Content);
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
            var doc = new HtmlDocument();
            doc.LoadHtml(await response.Content.ReadAsStringAsync());
            Assert.AreEqual("Galactic Developments - Die Geschichte der Zukunft", doc.DocumentNode.SelectNodes("//meta").FirstOrDefault(n => n.GetAttributeValue("property", "") == "og:title")?.GetAttributeValue("content", ""));
            Assert.AreEqual("Die Geschichte der Zukunft", doc.DocumentNode.SelectNodes("//h1").FirstOrDefault()?.GetDirectInnerText().Trim());
        }

        [TestMethod]
        [TestCategory("GaldevWeb.Pages")]
        public async Task About()
        {
            // Act
            var response = await _client.GetAsync("/About");

            // Assert
        }

    }
}
