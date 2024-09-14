using System.Xml;
using Maple2Storage.Types;

namespace GameParser.Parsers;

public static class TitleNameParser {
    public static readonly Dictionary<int, string> TitleNames = [];

    static TitleNameParser() {
        XmlDocument? xmlFile =
            Paths.XmlReader.GetXmlDocument(Paths.XmlReader.Files.First(x => x.Name.StartsWith("string/en/titlename.xml")));

        if (xmlFile is null) {
            throw new("Failed to load titlename.xml");
        }

        XmlNodeList? nodes = xmlFile.SelectNodes("/ms2/key");
        if (nodes is null) {
            throw new("Failed to load titlename.xml");
        }
        foreach (XmlNode node in nodes) {
            int id = int.Parse(node.Attributes!["id"]?.Value ?? throw new("Failed to load titlename.xml"));
            string name = node.Attributes["name"]?.Value ?? "";

            TitleNames[id] = name;
        }
    }
}
