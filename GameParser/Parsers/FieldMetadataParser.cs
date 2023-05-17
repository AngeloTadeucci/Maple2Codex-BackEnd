using System.Xml;
using Maple2Storage.Types;

namespace GameParser.Parsers;

public static class FieldMetadataParser
{
    public static readonly Dictionary<int, List<(string mapName, int mapId)>> FieldMetadata = new();

    static FieldMetadataParser()
    {
        XmlDocument? xmlFile = Paths.XmlReader.GetXmlDocument(Paths.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("table/na/fieldmetadata.xml")));

        if (xmlFile is null)
        {
            throw new("Failed to load fieldmetadata.xml");
        }

        XmlNodeList? nodes = xmlFile.SelectNodes("/ms2/npc");
        if (nodes is null)
        {
            throw new("Failed to load fieldmetadata.xml");
        }

        foreach (XmlNode node in nodes)
        {
            if (!int.TryParse(node.Attributes?["id"]?.Value, out int result))
            {
                continue;
            }

            // get nodes
            XmlNodeList? fieldNodes = node.SelectNodes("field");
            if (fieldNodes is null)
            {
                continue;
            }

            FieldMetadata.Add(result, new());

            foreach (XmlNode fieldNode in fieldNodes)
            {
                if (!int.TryParse(fieldNode.Attributes?["id"]?.Value, out int fieldId))
                {
                    continue;
                }

                if (!MapNameParser.MapNames.TryGetValue(fieldId, out string? mapName))
                {
                    mapName = "";
                }

                if (FieldMetadata[result].Any(x => x.mapId == fieldId))
                {
                    continue;
                }

                FieldMetadata[result].Add((mapName, fieldId));
            }
        }
    }
}