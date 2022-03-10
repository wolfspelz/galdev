using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace JsonPath.Test
{
    [TestClass]
    public class JsonPathContentProviderTest
    {
        [TestMethod]
        [TestCategory("JsonPathContentProviderTest")]
        public void Survive_null_lang()
        {
            // Arrange
            var sut = new JsonPath.TextProvider(new MemoryDataProvider(), "AppName", null, "TextName");

            // Act
            // Assert
            var s = sut.String(path: "path", i18n: new StringGeneratorI18n { ["de-DE"] = () => "a", ["en-US"] = () => "b", });

        }
    }
}
