using System.Xml;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;

namespace GameParser.Parsers;

public static class QuestNameParser
{
    public record QuestDescription(string Name, string Description, string Manual, string Complete);
    public static readonly Dictionary<int, QuestDescription> QuestNames = new();

    static QuestNameParser()
    {
        IEnumerable<PackFileEntry> xmlFiles = Paths.XmlReader.Files.Where(x => x.Name.StartsWith("string/en/questdescription_"));

        foreach (PackFileEntry packFile in xmlFiles)
        {
            XmlDocument? xmlFile = Paths.XmlReader.GetXmlDocument(packFile);
            if (xmlFile is null)
            {
                throw new("Failed to load xml file: " + packFile.Name);
            }

            XmlNodeList? nodes = xmlFile.SelectNodes("/ms2/key");
            if (nodes is null)
            {
                throw new("Failed to load xml file: " + packFile.Name);
            }
            foreach (XmlNode node in nodes)
            {
                int id = int.Parse(node.Attributes!["id"]?.Value ?? throw new("Failed to load xml file: " + packFile.Name));
                string name = node.Attributes["name"]?.Value ?? "";
                string description = node.Attributes["desc"]?.Value ?? "";
                string manual = node.Attributes["manual"]?.Value ?? "";
                string complete = node.Attributes["complete"]?.Value ?? "";
                
                QuestNames[id] = new(name, description, manual, complete);
            }
        }
    }
}