﻿namespace GaldevWeb
{
    public class GaldevConfig : MemoryCallbackConfig
    {
        public string AppName = "GaldevWeb";
        public string DataIndexPath = "../../../data/data.yaml";
        public string BlogIndexPath = "wwwroot/blog/blog.yaml";
        public string StageIndexPath = "wwwroot/images/stage/stage.yaml";
        //public string DataIndexPath = "../../../../../../data/data.yaml";
        //public string BlogIndexPath = "../../../../GaldevWeb/wwwroot/blog/blog.yaml";
        public string NotFoundImagePath = "wwwroot/images/site/NotFound.jpg";
        public int IndexListLimit = 20;
        public int ListMinTextLength = 300;
        public int EntryPageTextLength = 4000;
        public string GitHubProjectDataBaseUrl = "https://raw.githubusercontent.com/wolfspelz/galdev/main";
        public int WebpMemoryCacheDurationSec = 7 * 24 * 3600;
        public int MaxLogLines = 10_000;
        public List<string> LogLineRegexDenyList = new() { @"(?i)""agent"":""[^""]*?bot[^""]*?""", @"(?i)""agent"":""[^""]*?spider[^""]*?""" };
    }
}
