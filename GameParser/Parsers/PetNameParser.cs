using System.Xml;
using Maple2Storage.Types;

namespace GameParser.Parsers;

public static class PetNameParser
{
    public static readonly Dictionary<int, string> PetNames = new();

    static PetNameParser()
    {
        XmlDocument? xmlFile = Paths.XmlReader.GetXmlDocument(Paths.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("string/en/petname.xml")));

        if (xmlFile is null)
        {
            throw new("Failed to load petname.xml");
        }

        XmlNodeList? nodes = xmlFile.SelectNodes("/ms2/key");
        if (nodes is null)
        {
            throw new("Failed to load petname.xml");
        }
        foreach (XmlNode node in nodes)
        {
            int id = int.Parse(node.Attributes?["id"]?.Value ?? "0");
            string name = node.Attributes?["name"]?.Value ?? "";

            PetNames[id] = name;
        }
    }
}