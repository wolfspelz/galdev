namespace GaldevWeb
{
    public class BlogIndex : List<BlogPost>
    {
        public IDataProvider DataProvider = new TabConvertingFileDataProvider();
        public string IndexFilePath = "wwwroot/blog/index.yaml";
        public ICallbackLogger Log = new NullCallbackLogger();

        public void Load()
        {
            var indexData = DataProvider.GetData(IndexFilePath);
            var indexNode = JsonPath.Node.FromYaml(indexData, new YamlDeserializer.Options { LowerCaseDictKeys = true });

            var entriesNode = indexNode["entries"].AsDictionary;
            foreach (var entryNode in entriesNode) {
                var name = entryNode.Key;
                foreach (var fileByLang in entryNode.Value.AsDictionary) {
                    var folderPath = Path.GetDirectoryName(IndexFilePath) ?? "";
                    var fileName = fileByLang.Value.AsString;

                    var post = GetPostFromFile(name, folderPath, fileName);
                    if (post != null) {
                        this.Add(post);
                    }
                }
            }

        }

        private BlogPost? GetPostFromFile(string name, string folderPath, string fileName)
        {
            if (fileName.EndsWith(".yaml")) {
                fileName = fileName[..^".yaml".Length];
            }
            if (fileName.EndsWith(".yml")) {
                fileName = fileName[..^".yml".Length];
            }

            var entryPath = (folderPath + "/" + fileName).Replace("//", "/");
            var entryPathWithExt = entryPath + ".yaml";

            if (!DataProvider.HasData(entryPathWithExt)) {
                entryPathWithExt = entryPath + ".yml";
                if (!DataProvider.HasData(entryPathWithExt)) {
                    Log.Warning($"No entry file= {entryPathWithExt}");
                    return null;
                }
            }

            var contentData = DataProvider.GetData(entryPathWithExt);
            var contentNode = JsonPath.Node.FromYaml(contentData, new YamlDeserializer.Options { LowerCaseDictKeys = true });

            var post = new BlogPost();

            post.Name = name.ToLower();
            post.Language = contentNode["language"].AsString;
            post.Title = contentNode["title"].AsString;
            post.Time = contentNode["time"].AsString;
            post.Author = contentNode["author"].AsString;
            post.Summary = contentNode["summary"].AsString;
            post.Image = contentNode["image"].AsString;
            post.Html = contentNode["html"].AsString;
            post.Text = contentNode["text"].AsString;
            post.Tags = contentNode["tags"].AsList.Select(n => n.AsString).ToArray();

            return post;
        }

        public static string GetNameFromSeoTitle(string seoTitle)
        {
            var parts = seoTitle.ToLower().Split(new char[] { '-', ':' }, 2);
            return parts[0];
        }

        internal BlogPost? GetPost(string name, string lang)
        {
            name = name.ToLower();
            return this.FirstOrDefault(p => p.Name == name && p.Language == lang);
        }
    }
}