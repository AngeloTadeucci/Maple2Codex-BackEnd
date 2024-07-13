using System.Xml;
using Maple2Storage.Types;

namespace GameParser.Parsers;

public static class NpcNameParser {
    public static readonly Dictionary<int, string> NpcNames = [];
    public static readonly Dictionary<int, string> NpcNamesPlural = [];
    public static readonly Dictionary<int, string> NpcTitles = [];

    static NpcNameParser() {
        XmlDocument? xmlFile = Paths.XmlReader.GetXmlDocument(Paths.XmlReader.Files.First(x => x.Name.StartsWith("string/en/npcname.xml")));
        XmlDocument? xmlFilePlural =
            Paths.XmlReader.GetXmlDocument(Paths.XmlReader.Files.First(x => x.Name.StartsWith("string/en/npcnameplural.xml")));
        XmlDocument? xmlFileTitles = Paths.XmlReader.GetXmlDocument(Paths.XmlReader.Files.First(x => x.Name.StartsWith("string/en/npctitle.xml")));

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
            int id = int.Parse(node.Attributes?["id"]?.Value ?? "0");
            string name = node.Attributes?["name"]?.Value ?? "";

            NpcNames[id] = name;
        }

        nodes = xmlFilePlural.SelectNodes("/ms2/key");
        if (nodes is null) {
            throw new("Failed to load itemnameplural.xml");
        }
        foreach (XmlNode node in nodes) {
            int id = int.Parse(node.Attributes?["id"]?.Value ?? "0");
            string name = node.Attributes?["name"]?.Value ?? "";

            NpcNamesPlural[id] = name;
        }

        nodes = xmlFileTitles.SelectNodes("/ms2/key");
        if (nodes is null) {
            throw new("Failed to load npctitle.xml");
        }
        foreach (XmlNode node in nodes) {
            int id = int.Parse(node.Attributes?["id"]?.Value ?? "0");
            string name = node.Attributes?["name"]?.Value ?? "";

            NpcTitles[id] = name;
        }
    }
}
