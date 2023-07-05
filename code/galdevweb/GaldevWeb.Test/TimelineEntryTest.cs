namespace GaldevWeb.Test
{
    [TestClass]
    public class TimelineEntryTest
    {
        [TestMethod]
        [TestCategory("GaldevWeb")]
        public void DisplayName_chooses_wisely_in_the_order_ShortTitle_Headline0_Title()
        {
            // Arrange // Act // Assert
            Assert.AreEqual("ShortTitle", new TimelineEntry("Name", "1234", "Title Title Title", new[] { "a", "b" }) { ShortTitle = "ShortTitle" }.DisplayName);
            Assert.AreEqual("ShortTitle ShortTitle ShortTitle ShortTitle ShortTitle ShortTitle", new TimelineEntry("Name", "1234", "Title Title Title ", new[] { "a", "b" }) { ShortTitle = "ShortTitle ShortTitle ShortTitle ShortTitle ShortTitle ShortTitle" }.DisplayName);
            Assert.AreEqual("Title Title Title", new TimelineEntry("Name", "1234", "Title Title Title", new[] { "a", "b" }) { }.DisplayName);
            Assert.AreEqual("Headline", new TimelineEntry("Name", "1234", "Title Title Title", new[] { "a", "b" }) { Headline = "Headline: Subheadline" }.DisplayName);
            Assert.AreEqual("Title Title Title", new TimelineEntry("Name", "1234", "Title Title Title", new[] { "a", "b" }) { Headline = "Headline Headline Headline Headline: Subheadline" }.DisplayName);
            Assert.AreEqual("Title Title Title", new TimelineEntry("Name", "1234", "Title Title Title", new[] { "a", "b" }) { Headline = "HEADLINE: Subheadline" }.DisplayName);
            //Assert.AreEqual("", new TimelineEntry("name", "1234", "title title title", new[] { "a", "b" }) { ShortTitle = "123456789 123456789 123456789 ", Headline = "123456789 123456789 123456789 123456789 : 123456789 123456789 123456789 ", }.DisplayName);
        }

    }
}
