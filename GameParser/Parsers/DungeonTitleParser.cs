using System.Xml;
using Maple2Storage.Types;

namespace GameParser.Parsers;

public static class DungeonTitleParser {
    public static readonly Dictionary<int, (string name, string uiDescription)> DungeonTitleNames = [];

    static DungeonTitleParser() {
        XmlDocument? xmlFile =
            Paths.XmlReader.GetXmlDocument(Paths.XmlReader.Files.First(x => x.Name.StartsWith("string/en/stringfieldenterance.xml")));

        if (xmlFile is null) {
            throw new("Failed to load stringfieldenterance.xml");
        }

        XmlNodeList? nodes = xmlFile.SelectNodes("/ms2/key");
        if (nodes is null) {
            throw new("Failed to load stringfieldenterance.xml");
        }
        foreach (XmlNode node in nodes) {
            int id = int.Parse(node.Attributes?["id"]?.Value ?? "0");
            string name = node.Attributes?["name"]?.Value ?? "";
            string uiDescription = node.Attributes?["uiDescription"]?.Value ?? "";

            DungeonTitleNames[id] = (name, uiDescription);
        }
    }
}
