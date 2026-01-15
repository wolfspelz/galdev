namespace GaldevWeb.Test;

[TestClass]
public class TimelineSeriesTest
{
    [TestMethod]
    [TestCategory("GaldevWeb")]
    public void CreateAliases_creates_aliases_for_language_dependent_indexing()
    {
        // Arrange
        var sut = new TimelineSeries();
        Array.ForEach(
            new[] { "3", "2", "4", "1", "5", "3b" },
            x => {
                var entry = new TimelineEntry(x, x, x, x + ".yaml", new[] { x, x }, x) {
                    Aliases = new[] { x + x }
                };
                sut.Add(x, entry);
            });

        // Act
        sut.CreateAliases();

        // Assert
        Assert.AreEqual("1", sut.GetEntry("1")?.Name);
        Assert.AreEqual("1", sut.GetEntry("11")?.Name);
    }

    [TestMethod]
    [TestCategory("GaldevWeb")]
    public void CreateTopics_creates_a_TopicSet()
    {
        // Arrange
        var sut = new TimelineSeries();
        Array.ForEach(
            new[] { "3", "2", "4", "1", "5", "3b" },
            x => {
                var entry = new TimelineEntry(x, x, x, x + ".yaml", new[] { x, x }, x) {
                    Topics = new[] { x, "t" }
                };
                sut.Add(x, entry);
            });
        sut.ConnectEntries();

        // Act
        sut.CreateTopics(new Dictionary<string, string> { ["1"] = "1", ["t"] = "t", ["3"] = "3" });

        // Assert
        Assert.AreEqual(1, sut.GetEntriesOfTopic("1")?.Count);
        Assert.AreEqual("1", sut.GetEntriesOfTopic("1")?[0].Name);

        Assert.AreEqual(1, sut.GetEntriesOfTopic("3")?.Count);
        Assert.AreEqual("3", sut.GetEntriesOfTopic("3")?[0].Name);

        Assert.AreEqual(6, sut.GetEntriesOfTopic("t")?.Count);
        Assert.AreEqual("1", sut.GetEntriesOfTopic("t")?[0].Name);
        Assert.AreEqual("5", sut.GetEntriesOfTopic("t")?[5].Name);
    }

    [TestMethod]
    [TestCategory("GaldevWeb")]
    public void ConnectEntries_inserts_Previous_and_Next_Properties_into_all_entries()
    {
        // Arrange
        var sut = new TimelineSeries();
        Array.ForEach(
            new[] { "3", "2", "4", "1", "5", "3b" },
            x => sut.Add(x, new TimelineEntry(x, x, x, x + ".yaml", new[] { x, x }, x))
            );

        // Act
        sut.ConnectEntries();

        // Assert
        Assert.AreEqual("", sut.GetEntry("1")?.Previous);
        Assert.AreEqual("1", sut.GetEntry("2")?.Previous);
        Assert.AreEqual("2", sut.GetEntry("3")?.Previous);
        Assert.AreEqual("3", sut.GetEntry("3b")?.Previous);
        Assert.AreEqual("3b", sut.GetEntry("4")?.Previous);
        Assert.AreEqual("4", sut.GetEntry("5")?.Previous);

        Assert.AreEqual("2", sut.GetEntry("1")?.Next);
        Assert.AreEqual("3", sut.GetEntry("2")?.Next);
        Assert.AreEqual("3b", sut.GetEntry("3")?.Next);
        Assert.AreEqual("4", sut.GetEntry("3b")?.Next);
        Assert.AreEqual("5", sut.GetEntry("4")?.Next);
        Assert.AreEqual("", sut.GetEntry("5")?.Next);

    }

}
