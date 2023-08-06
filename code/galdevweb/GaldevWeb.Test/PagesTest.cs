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
        public async Task Index_de()
        {
            // Act
            var response = await _client.GetAsync("/Index?lang=de-DE");

            // Assert
            Assert.IsNotNull(response.Content);
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
            var doc = new HtmlDocument();
            doc.LoadHtml(await response.Content.ReadAsStringAsync());
            Assert.AreEqual("Galactic Developments - Die Geschichte der Zukunft", doc.DocumentNode.SelectNodes("//meta").FirstOrDefault(n => n.GetAttributeValue("property", "") == "og:title")?.GetAttributeValue("content", ""));
            Assert.AreEqual("Die Geschichte der Zukunft", doc.DocumentNode.SelectNodes("//h1").FirstOrDefault()?.InnerText.Trim());
        }

        [TestMethod]
        [TestCategory("GaldevWeb.Pages")]
        public async Task Index_en()
        {
            // Act
            var response = await _client.GetAsync("/Index?lang=en-US");

            // Assert
            Assert.IsNotNull(response.Content);
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
            var doc = new HtmlDocument();
            doc.LoadHtml(await response.Content.ReadAsStringAsync());
            Assert.AreEqual("Galactic Developments - The History of the Future", doc.DocumentNode.SelectNodes("//meta").FirstOrDefault(n => n.GetAttributeValue("property", "") == "og:title")?.GetAttributeValue("content", ""));
            Assert.AreEqual("The History of the Future", doc.DocumentNode.SelectNodes("//h1").FirstOrDefault()?.InnerText.Trim());
        }

        [TestMethod]
        [TestCategory("GaldevWeb.Pages")]
        public async Task About()
        {
            // Act
            var response = await _client.GetAsync("/About");

            // Assert
            Assert.IsNotNull(response.Content);
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [TestMethod]
        [TestCategory("GaldevWeb.Pages")]
        public async Task Timeline_de()
        {
            // Act
            var response = await _client.GetAsync("/Timeline/lunarrevolt-whatever?lang=de-DE");

            // Assert
            Assert.IsNotNull(response.Content);
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
            var doc = new HtmlDocument();
            doc.LoadHtml(await response.Content.ReadAsStringAsync());
            Assert.AreEqual("Aufstand in lunaren Strafkolonien", doc.DocumentNode.SelectNodes("//meta").FirstOrDefault(n => n.GetAttributeValue("property", "") == "og:title")?.GetAttributeValue("content", ""));
            Assert.AreEqual("2197 Aufstand in lunaren Strafkolonien", doc.DocumentNode.SelectNodes("//h3").FirstOrDefault()?.InnerText.Trim());
        }

        [TestMethod]
        [TestCategory("GaldevWeb.Pages")]
        public async Task Timeline_en()
        {
            // Act
            var response = await _client.GetAsync("/Timeline/lunarrevolt-whatever?lang=en-US");

            // Assert
            Assert.IsNotNull(response.Content);
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
            var doc = new HtmlDocument();
            doc.LoadHtml(await response.Content.ReadAsStringAsync());
            Assert.AreEqual("Rebellion in Lunar Penal Colonies", doc.DocumentNode.SelectNodes("//meta").FirstOrDefault(n => n.GetAttributeValue("property", "") == "og:title")?.GetAttributeValue("content", ""));
            Assert.AreEqual("2197 Rebellion in Lunar Penal Colonies", doc.DocumentNode.SelectNodes("//h3").FirstOrDefault()?.InnerText.Trim());
        }

        [TestMethod]
        [TestCategory("GaldevWeb.Pages")]
        public async Task Blog_de()
        {
            // Act
            var response = await _client.GetAsync("/Blog?lang=de-DE");

            // Assert
            Assert.IsNotNull(response.Content);
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
            var doc = new HtmlDocument();
            doc.LoadHtml(await response.Content.ReadAsStringAsync());
            Assert.AreEqual("News", doc.DocumentNode.SelectNodes("//h1").FirstOrDefault()?.InnerText.Trim());
        }

    }
}
