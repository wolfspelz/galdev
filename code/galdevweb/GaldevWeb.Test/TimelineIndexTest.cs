namespace GaldevWeb.Test
{
    [TestClass]
    public class TimelineIndexTest
    {
        [TestMethod]
        [TestCategory("GaldevWeb")]
        public void Load_loads_2_languages_with_2_entries_each()
        {
            // Arrange
            var sut = new TimelineIndex();
            sut.IndexFilePath = "timeline/index.yaml";
            sut.DataProvider = new MemoryDataProvider {
                Data = new Dictionary<string, string> {
                    [sut.IndexFilePath] = @"
languages:
    en:
        path: en
        images: en/images
        topics:
            a: AE
            b: BE
    de:
        path: de
        images: de/images
        topics:
            a: AD
            b: BD
entries:
    a:
        en: a
        de: a.yaml
    b:
        en: b
        de: b.yaml
",
                    ["timeline/en/a.yaml"] = @"
year: 1234
title: a en title
text: |
    a en text
    abc
    def
",
                    ["timeline/de/a.yaml"] = @"
year: 1234
title: a de title
text: |
    a de text
    abc
    def
",
                    ["timeline/en/b.yaml"] = @"
year: 1235
title: b en title
text: |
    b en text
    abc
    def
",
                    ["timeline/de/b.yaml"] = @"
year: 1235
title: b de title
text: |
    b de text
    abc
    def
",
                }
            };

            // Act
            sut.Load();

            // Assert
            Assert.AreEqual("a", sut.GetEntry("a", "en").Name);
            Assert.AreEqual("1234", sut.GetEntry("a", "en").Year);
            Assert.AreEqual("a en title", sut.GetEntry("a", "en").Title);

            Assert.AreEqual("a", sut.GetEntry("a", "de").Name);
            Assert.AreEqual("1234", sut.GetEntry("a", "de").Year);
            Assert.AreEqual("a de title", sut.GetEntry("a", "de").Title);

            Assert.AreEqual("b", sut.GetEntry("b", "en").Name);
            Assert.AreEqual("1235", sut.GetEntry("b", "en").Year);
            Assert.AreEqual("b en title", sut.GetEntry("b", "en").Title);

            Assert.AreEqual("b", sut.GetEntry("b", "de").Name);
            Assert.AreEqual("1235", sut.GetEntry("b", "de").Year);
            Assert.AreEqual("b de title", sut.GetEntry("b", "de").Title);
        }

    }
}
