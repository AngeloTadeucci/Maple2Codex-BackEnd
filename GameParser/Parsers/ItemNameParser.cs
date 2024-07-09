using System.Xml;
using Maple2Storage.Types;

namespace GameParser.Parsers;

public static class ItemNameParser {
    public static readonly Dictionary<int, string> ItemNames = [];
    public static readonly Dictionary<int, string> ItemNamesPlural = [];

    static ItemNameParser() {
        XmlDocument? xmlFile = Paths.XmlReader.GetXmlDocument(Paths.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("string/en/itemname.xml")));
        XmlDocument? xmlFilePlural =
            Paths.XmlReader.GetXmlDocument(Paths.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("string/en/itemnameplural.xml")));

        if (xmlFile is null) {
            throw new("Failed to load itemname.xml");
        }

        if (xmlFilePlural is null) {
            throw new("Failed to load itemnameplural.xml");
        }

        XmlNodeList? nodes = xmlFile.SelectNodes("/ms2/key");
        if (nodes is null) {
            throw new("Failed to load itemname.xml");
        }
        foreach (XmlNode node in nodes) {
            int id = int.Parse(node.Attributes?["id"]?.Value ?? throw new("Failed to load itemname.xml"));
            string name = node.Attributes["name"]?.Value ?? "";

            ItemNames[id] = name;
        }

        nodes = xmlFilePlural.SelectNodes("/ms2/key");
        if (nodes is null) {
            throw new("Failed to load itemnameplural.xml");
        }
        foreach (XmlNode node in nodes) {
            int id = int.Parse(node.Attributes?["id"]?.Value ?? throw new("Failed to load itemnameplural.xml"));
            string name = node.Attributes["name"]?.Value ?? "";

            ItemNamesPlural[id] = name;
        }
    }
}
