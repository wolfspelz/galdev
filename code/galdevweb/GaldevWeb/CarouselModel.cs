namespace GaldevWeb
{
    public class CarouselItem
    {
        public string Link = "";
        public string Image = "";
        public string Description = "";
    }

    public class CarouselModel : List<CarouselItem>
    {
        internal void Load(TimelineSeries timeline)
        {
            foreach (var kv in timeline) {
                var name = kv.Key;
                var entry = kv.Value;
                if (entry.Tags.Contains("_carousel", StringComparer.OrdinalIgnoreCase) && Is.Value(entry.Image)) {
                    var item = new CarouselItem {
                        Link = entry.SeoTitle,
                        Image = entry.Image ?? "",
                        Description = entry.Description,
                    };
                    this.Add(item);
                }
            }
        }
    }

}