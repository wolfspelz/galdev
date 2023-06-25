namespace GaldevWeb
{
    public class GaldevConfig : MemoryCallbackConfig
    {
        public string AppName = "GaldevWeb";
        public string IndexPath = "../../../data/index.yaml";
        public string NotFoundImagePath = "wwwroot/images/NotFound.jpg";
        public int ListMinTextLength = 1000;
    }
}
