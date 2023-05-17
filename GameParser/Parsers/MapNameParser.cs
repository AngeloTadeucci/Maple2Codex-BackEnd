using System.Xml;
using Maple2Storage.Types;

namespace GameParser.Parsers;

public static class MapNameParser
{
    public static readonly Dictionary<int, string> MapNames = new();

    static MapNameParser()
    {
        XmlDocument? xmlFile = Paths.XmlReader.GetXmlDocument(Paths.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("string/en/mapname.xml")));

        if (xmlFile is null)
        {
            throw new("Failed to load mapname.xml");
        }

        XmlNodeList? nodes = xmlFile.SelectNodes("/ms2/key");
        if (nodes is null)
        {
            throw new("Failed to load mapname.xml");
        }

        foreach (XmlNode node in nodes)
        {
            int id = int.Parse(node.Attributes?["id"]?.Value ?? "0");
            string name = node.Attributes?["name"]?.Value ?? "";

            MapNames[id] = name;
        }
    }
}