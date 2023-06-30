
namespace GaldevWeb.Test
{
    [TestClass]
    public class TimelineSeriesTest
    {
        [TestMethod]
        [TestCategory("GaldevWeb")]
        public void ConnectEntries_inserts_Previous_and_Next_Properties_into_all_entries()
        {
            // Arrange
            var sut = new TimelineSeries();
            Array.ForEach(
                new[] { "3", "2", "4", "1", "5", "3b" },
                x => sut.Add(x, new TimelineEntry(x, x, x, new[] { x, x }))
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
}
