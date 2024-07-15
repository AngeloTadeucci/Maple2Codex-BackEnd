using System.Xml;
using GameParser.DescriptionHelper;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;

namespace GameParser.Parsers;

public static class QuestNameParser {
    public record QuestDescription(string Name, string Description, string Manual, string Complete);
    public static readonly Dictionary<int, QuestDescription> QuestNames = [];

    static QuestNameParser() {
        IEnumerable<PackFileEntry> xmlFiles = Paths.XmlReader.Files.Where(x => x.Name.StartsWith("string/en/questdescription_"));

        foreach (PackFileEntry packFile in xmlFiles) {
            XmlDocument? xmlFile = Paths.XmlReader.GetXmlDocument(packFile);
            if (xmlFile is null) {
                throw new("Failed to load xml file: " + packFile.Name);
            }

            XmlNodeList? nodes = xmlFile.SelectNodes("/ms2/quest");
            if (nodes is null || nodes.Count == 0) {
                continue;
            }
            foreach (XmlNode node in nodes) {
                int id = int.Parse(node.Attributes?["questID"]?.Value ?? "0");
                string name = node.Attributes?["name"]?.Value ?? "";
                string description = node.Attributes?["desc"]?.Value ?? "";
                string manual = node.Attributes?["manual"]?.Value ?? "";
                string complete = node.Attributes?["complete"]?.Value ?? "";
                if (id == 0) {
                    continue;
                }

                QuestNames[id] = new(Helper.FixDescription(name), Helper.FixDescription(description), Helper.FixDescription(manual), Helper.FixDescription(complete));
            }
        }
    }
}
