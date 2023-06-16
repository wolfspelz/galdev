namespace JsonPath
{
    public static class YamlDeserializer
    {
        public static Action? Dont { get; set; }

        public static Node Decode(string yaml)
        {
            if (string.IsNullOrEmpty(yaml)) {
                return new Node(Node.Type.Empty);
            }

            return Node.FromJson(yaml);
        }
    }
}
