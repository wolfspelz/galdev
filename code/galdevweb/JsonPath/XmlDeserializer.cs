using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace JsonPath
{
    public class Xml
    {
        public const string Name = "Name";
        public const string Attributes = "Attributes";
        public const string Text = "Text";
        public const string Children = "Children";
    }

    public class XmlDeserializerOptions
    {
        public string Name = Xml.Name;
        public string Attributes = Xml.Attributes;
        public string Text = Xml.Text;
        public string Children = Xml.Children;
        public bool FlattenAttributes = false;
        public bool TextNodesAsChildren = true;
    }

    public class XmlDeserializer
    {
        public Node Parse(string xml, XmlDeserializerOptions? options = null)
        {
            options = options ?? new XmlDeserializerOptions();
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var root = doc.DocumentElement;
            if (root != null) {
                return GetNode(root, options);
            }

            return new Node(Node.Type.Dictionary);
        }

        private Node GetNode(XmlNode xmlNode, XmlDeserializerOptions options)
        {
            var node = new Node(Node.Type.Dictionary).AsDictionary;
            node.Add(options.Name, xmlNode.Name);

            if (options.FlattenAttributes) {
                foreach (XmlAttribute attr in xmlNode.Attributes) {
                    node.Add(attr.Name, attr.Value);
                }
            } else {
                var attributesDict = new Node(Node.Type.Dictionary).AsDictionary;
                foreach (XmlAttribute attr in xmlNode.Attributes) {
                    attributesDict.Add(attr.Name, attr.Value);
                }
                node.Add(options.Attributes, attributesDict);
            }

            var childList = new Node(Node.Type.Dictionary).AsList;
            var textBuilder = new StringBuilder();
            foreach (XmlNode child in xmlNode.ChildNodes) {
                if (child.NodeType == XmlNodeType.Element) {
                    var childNode = GetNode(child, options);
                    childList.Add(childNode);
                } else if (child.NodeType == XmlNodeType.Text) {
                    if (options.TextNodesAsChildren) {
                        childList.Add(child.Value);
                    }
                    textBuilder.Append(child.Value);
                }
            }
            node.Add(options.Children, childList);
            node.Add(options.Text, textBuilder.ToString());

            return node;
        }
    }
}
