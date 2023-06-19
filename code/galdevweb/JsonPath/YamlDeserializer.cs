using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace JsonPath
{
    public static class YamlDeserializer
    {
        public class Options
        {
            public bool LowerCaseDictKeys = false;
        }

        public static Node Deserialize(string yaml, Options? options = null)
        {
            if (string.IsNullOrEmpty(yaml)) {
                return new Node(Node.Type.Empty);
            }

            var deserializer = new YamlDotNet.Serialization.DeserializerBuilder().Build();
            dynamic parsed = deserializer.Deserialize<ExpandoObject>(yaml);

            return NodeFromDynamic(parsed, options ?? new Options());
        }

        private static Node NodeFromDynamic(dynamic obj, Options options)
        {
            if (obj == null) {
                return new Node(Node.Type.Empty);
            }

            if (obj is System.Dynamic.ExpandoObject expando) {
                var node = new Node(Node.Type.Dictionary);
                foreach (var pair in expando) {
                    var mappedKey = options.LowerCaseDictKeys ? pair.Key.ToLower() : pair.Key;
                    if (pair.Value != null) {
                        node.Dictionary.Add(mappedKey, NodeFromDynamic(pair.Value, options));
                    }
                }
                return node;
            }

            if (obj is string stringValue) {
                return Node.From(stringValue == null ? "" : stringValue);
            } else if (obj is int intValue) {
                return Node.From(intValue);
            } else if (obj is long longValue) {
                return Node.From(longValue);
            } else if (obj is float floatValue) {
                return Node.From(floatValue);
            } else if (obj is double doubleValue) {
                return Node.From(doubleValue);
            } else if (obj is bool boolValue) {
                return Node.From(boolValue);
            } else if (obj is DateTime dateValue) {
                return Node.From(dateValue);
            } else if (obj is Dictionary<object, object> dict) {
                var node = new Node(Node.Type.Dictionary);
                foreach (var pair in dict) {
                    var key = (string)pair.Key;
                    var mappedKey = options.LowerCaseDictKeys ? key.ToLower() : key;
                    node.Dictionary.Add(mappedKey, NodeFromDynamic(pair.Value, options));
                }
                return node;
            } else if (obj is List<object> list) {
                var node = new Node(Node.Type.List);
                foreach (var item in list) {
                    node.List.Add(NodeFromDynamic(item, options));
                }
                return node;
            } else {
                var t = obj.GetType();
                var s = t;
            }

            //if (obj is JValue jv) {
            //    switch (jv.Type) {
            //        case JTokenType.Comment: return new Node(Node.Type.Empty);
            //        case JTokenType.Integer: return Node.From(jv.Value == null ? 0L : (long)jv.Value);
            //        case JTokenType.Float: return Node.From(jv.Value == null ? 0.0D : (double)jv.Value);
            //        case JTokenType.String: return Node.From(jv.Value == null ? "" : (string)jv.Value);
            //        case JTokenType.Boolean: return Node.From(jv.Value == null ? false : (bool)jv.Value);
            //        case JTokenType.Null: return new Node(Node.Type.Empty);
            //        case JTokenType.Undefined: return new Node(Node.Type.Empty);
            //        case JTokenType.Guid: return Node.From(jv.Value == null ? "" : (string)jv.Value);
            //        case JTokenType.Uri: return Node.From(jv.Value == null ? "" : (string)jv.Value);
            //        case JTokenType.Date: return Node.From(jv.Value == null ? DateTime.MinValue : (DateTime)jv.Value);
            //        // ReSharper disable once RedundantCaseLabel
            //        case JTokenType.None:
            //        // ReSharper disable once RedundantCaseLabel
            //        case JTokenType.Object:
            //        // ReSharper disable once RedundantCaseLabel
            //        case JTokenType.Array:
            //        // ReSharper disable once RedundantCaseLabel
            //        case JTokenType.Constructor:
            //        // ReSharper disable once RedundantCaseLabel
            //        case JTokenType.Property:
            //        // ReSharper disable once RedundantCaseLabel
            //        case JTokenType.Raw:
            //        // ReSharper disable once RedundantCaseLabel
            //        case JTokenType.Bytes:
            //        // ReSharper disable once RedundantCaseLabel
            //        case JTokenType.TimeSpan:
            //        default:
            //            throw new Exception("Json.NET JToken.Type=" + jv.Type.ToString() + " not supported");
            //    }
            //}

            //if (obj is JArray list) {
            //    var node = new Node(Node.Type.List);
            //    foreach (var item in list) {
            //        node.List.Add(NodeFromDynamic(item, options));
            //    }
            //    return node;
            //}

            //if (obj is JObject dict) {
            //    var node = new Node(Node.Type.Dictionary);
            //    foreach (var pair in dict) {
            //        var key = options.LowerCaseKeys ? pair.Key.ToLower() : pair.Key;
            //        node.Dictionary.Add(key, NodeFromDynamic(pair.Value ?? JToken.Parse("''"), options));
            //    }
            //    return node;
            //}

            return new Node(Node.Type.Empty);
        }
    }
}
