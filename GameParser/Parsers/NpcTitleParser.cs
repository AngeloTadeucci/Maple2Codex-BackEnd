using System.Xml;
using Maple2Storage.Types;

namespace GameParser.Parsers;

public static class NpcTitleParser {
    public static readonly Dictionary<int, string> NpcTitle = [];

    static NpcTitleParser() {
        XmlDocument? xmlFile = Paths.XmlReader.GetXmlDocument(Paths.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("string/en/npctitle.xml")));

        if (xmlFile is null) {
            throw new("Failed to load npctitle.xml");
        }

        XmlNodeList? nodes = xmlFile.SelectNodes("/ms2/key");
        if (nodes is null) {
            throw new("Failed to load npctitle.xml");
        }
        foreach (XmlNode node in nodes) {
            if (!int.TryParse(node.Attributes?["id"]?.Value, out int id)) {
                continue;
            }

            string name = node.Attributes["name"]?.Value ?? "";

            if (string.IsNullOrEmpty(name)) {

            }

            NpcTitle.Add(id, name);
        }
    }
}
