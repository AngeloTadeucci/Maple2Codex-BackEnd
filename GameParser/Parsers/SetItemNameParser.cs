using System.Xml;
using Maple2Storage.Types;

namespace GameParser.Parsers;

public static class SetItemNameParser {
    private static readonly Dictionary<int, string> SetItemName = [];

    static SetItemNameParser() {
        XmlDocument? xmlFile =
            Paths.XmlReader.GetXmlDocument(Paths.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("string/en/setitemname.xml")));

        if (xmlFile is null) {
            throw new("Failed to load setitemname.xml");
        }

        XmlNodeList? nodes = xmlFile.SelectNodes("/ms2/key");
        if (nodes is null) {
            throw new("Failed to load setitemname.xml");
        }
        foreach (XmlNode node in nodes) {
            int id = int.Parse(node.Attributes!["id"]?.Value ?? throw new("Failed to load setitemname.xml"));
            string name = node.Attributes["name"]?.Value ?? "";

            SetItemName[id] = name;
        }
    }

    public static string GetSetItemName(int id) {
        return SetItemName.TryGetValue(id, out string? name) ? name : "";
    }
}
