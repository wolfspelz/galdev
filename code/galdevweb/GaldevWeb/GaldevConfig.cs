namespace GaldevWeb
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
        public int ListMinTextLength = 300;
        public int EntryPageTextLength = 4000;
        public string GitHubProjectDataBaseUrl = "https://raw.githubusercontent.com/wolfspelz/galdev/main";
        public int WebpMemoryCacheDurationSec = 7 * 24 * 3600;
    }
}
