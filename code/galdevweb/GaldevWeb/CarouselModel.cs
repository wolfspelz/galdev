namespace GaldevWeb
{
    public class CarouselItem
    {
        public string Name = "";
        public string Image = "";
        public string Description = "";
    }

    public class CarouselModel : List<CarouselItem>
    {
        public string IndexFilePath = "wwwroot/carousel/index.yaml";

        internal void Load()
        {
            var data = new TabConvertingFileDataProvider().GetData(IndexFilePath);
            var node = JsonPath.Node.FromYaml(data, new YamlDeserializer.Options { LowerCaseDictKeys = true });

            var entriesNode = node["entries"].AsList;
            foreach (var entryNode in entriesNode) {
                var item = new CarouselItem();
                item.Name = entryNode["name"].AsString;
                item.Image = entryNode["image"].AsString;
                item.Description = entryNode["description"].AsString;
                this.Add(item);
            }
        }
    }

}