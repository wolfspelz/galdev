namespace GaldevWeb
{
    public class GaldevConfig : MemoryCallbackConfig
    {
        public string AppName = "GaldevWeb";
        public string DataIndexPath = "../../../data/index.yaml";
        public string BlogIndexPath = "wwwroot/blog/index.yaml";
        //public string DataIndexPath = "../../../../../../data/index.yaml";
        //public string BlogIndexPath = "../../../../GaldevWeb/wwwroot/blog/index.yaml";
        public string NotFoundImagePath = "wwwroot/images/site/NotFound.jpg";
        public int ListMinTextLength = 1000;
        public int EntryPageTextLength = 4000;
    }
}
