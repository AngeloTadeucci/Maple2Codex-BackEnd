using System.Xml;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;

namespace GameParser.Parsers;

public static class SkillNameParser
{
    public static readonly Dictionary<int, string> SkillNames = new();

    static SkillNameParser()
    {
        IEnumerable<PackFileEntry> xmlFiles = Paths.XmlReader.Files.Where(x => x.Name.StartsWith("string/en/skillname_"));

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
                int id = int.Parse(node.Attributes?["id"]?.Value ?? throw new("Failed to load xml file: " + packFile.Name));
                string name = node.Attributes["name"]?.Value ?? "";

                SkillNames[id] = name;
            }
        }
    }
}