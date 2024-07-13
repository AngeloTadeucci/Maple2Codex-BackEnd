using System.Xml;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;

namespace GameParser.Parsers;

public static class StringCommonParser {
    private static readonly Dictionary<string, string> Strings = [];
    static StringCommonParser() {
        XmlDocument? xmlFile = Paths.XmlReader.GetXmlDocument(Paths.XmlReader.Files.First(x => x.Name.StartsWith("string/en/stringcommon.xml")));

        if (xmlFile is null) {
            throw new("Failed to load stringcommom.xml");
        }

        XmlNodeList? nodes = xmlFile.SelectNodes("/ms2/key");
        if (nodes is null) {
            throw new("Failed to load stringcommom.xml");
        }
        foreach (XmlNode node in nodes) {
            string id = node.Attributes?["id"]?.Value ?? "";
            string description = node.Attributes?["value"]?.Value ?? "";
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(description)) {
                continue;
            }

            Strings[id] = description;
        }
    }

    public static bool Get(string id, out string? description) {
        return Strings.TryGetValue(id, out description);
    }
}
