using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace galdevtool.Test
{
    [TestClass]
    public class ExportImportTest
    {
        [TestMethod]
        public void compare_all()
        {
            var x = Directory.GetCurrentDirectory();
            var bigfileData = File.ReadAllText(@"..\..\..\data\ExportImportTest\bigfile\data2.txt");
            var b2y = new Bigfile2Yaml();
            var y2b = new Yaml2Bigfile();

            var exportedTimeline = b2y.Analyse(bigfileData);
            var importedYamlData = y2b.Read(@"..\..\..\data\ExportImportTest\yaml");
            var importedTimeline = y2b.ProcessInput(importedYamlData);
            for (var i = 0; i < importedTimeline.Count; i++)
            {
                Assert.IsTrue(CompareGoodEnough(importedTimeline[i], exportedTimeline[i]), $"{importedTimeline[i].Year} exportedTimeline/importedTimeline different");
                //Assert.IsTrue(importedTimeline[i].Equals(exportedTimeline[i]), $"{importedTimeline[i].Year} exportedTimeline/importedTimeline different");
            }

            var years = new Dictionary<string, int>();
            var exportedYamlData = exportedTimeline.Select(t => new KeyValuePair<string, string>(b2y.GetFilePath(t, @"..\..\..\data\ExportImportTest\yaml", years), b2y.CreateYamlData(t))).ToDictionary(kv => kv.Key, kv => kv.Value);
            var importedYamlDataKeys = importedYamlData.Keys.ToList();
            var importedYamlDataValues = importedYamlData.Values.ToList();
            var exportedYamlDataKeys = exportedYamlData.Keys.ToList();
            var exportedYamlDataValues = exportedYamlData.Values.ToList();
            for (var i = 0; i < importedYamlDataKeys.Count; i++)
            {
                Assert.IsTrue(importedYamlDataKeys[i] == exportedYamlDataKeys[i]);
            }
            for (var i = 0; i < importedYamlDataValues.Count; i++)
            {
                //Assert.IsTrue(importedYamlDataValues[i] == exportedYamlDataValues[i]);
            }

            (var generatedBigfileData, var copyFiles) = y2b.ProcessOutput(importedTimeline, "", "", "");
            Assert.AreEqual(bigfileData, generatedBigfileData);
        }

        private bool CompareGoodEnough(TimelineEntry e1, TimelineEntry e2)
        {
            var x = e1;
            var y = e2;
            if (y == null) return false;
            if (x.Name != y.Name) return false;
            if (x.Year != y.Year) return false;
            if (x.Title != y.Title) return false;
            if (x.Short != y.Short) return false;
//            if (x.Summary != y.Summary) return false;
            if (x.Headline != y.Headline) return false;
            if (x.Image != y.Image) return false;
            if (x.Smallimage != y.Smallimage) return false;
            //if (x.Twitter != y.Twitter) return false;
            //if (x.Twitterimage != y.Twitterimage) return false;
            //if (x.Facebook != y.Facebook) return false;
            //if (x.Facebook2 != y.Facebook2) return false;
            //if (x.Facebook3 != y.Facebook3) return false;
            //if (x.Facebookimage != y.Facebookimage) return false;
            if (x.Post != y.Post) return false;
            if (x.Postimage != y.Postimage) return false;
            if (x.Author != y.Author) return false;
            if (x.Translation != y.Translation) return false;
            if (string.Join("", x.Tags) != string.Join("", y.Tags)) return false;
            if (string.Join("", x.Topics) != string.Join("", y.Topics)) return false;
            if (string.Join("", x.Text) != string.Join("", y.Text)) return false;
            return true;
        }
    }
}
