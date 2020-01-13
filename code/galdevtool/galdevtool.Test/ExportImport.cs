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
                Assert.IsTrue(importedTimeline[i].Equals(exportedTimeline[i]));
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
                Assert.IsTrue(importedYamlDataValues[i] == exportedYamlDataValues[i]);
            }

            (var generatedBigfileData, var copyFiles) = y2b.ProcessOutput(importedTimeline, "", "", "");
            Assert.AreEqual(bigfileData, generatedBigfileData);
        }
    }
}
