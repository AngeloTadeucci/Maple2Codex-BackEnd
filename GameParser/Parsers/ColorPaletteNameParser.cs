using System.Xml;
using GameParser.DescriptionHelper;
using Maple2Storage.Types;

namespace GameParser.Parsers;

public static class ColorPaletteNameParser {
    public static readonly Dictionary<int, string> ColorPaletteNames = [];

    static ColorPaletteNameParser() {
        XmlDocument? xmlFile =
            Paths.XmlReader.GetXmlDocument(Paths.XmlReader.Files.First(x => x.Name.StartsWith("string/en/stringcolorpalette.xml")));

        if (xmlFile is null) {
            throw new("Failed to load stringcolorpalette.xml");
        }

        XmlNodeList? nodes = xmlFile.SelectNodes("/ms2/key");
        if (nodes is null) {
            throw new("Failed to load stringcolorpalette.xml");
        }

        foreach (XmlNode node in nodes) {
            int id = int.Parse(node.Attributes!["id"]?.Value ?? throw new("Failed to load stringcolorpalette.xml"));
            string name = node.Attributes["name"]?.Value ?? "";
            name = Helper.FixDescription(name);

            ColorPaletteNames[id] = name;
        }
    }

    public static string GetSetItemName(int id) {
        return ColorPaletteNames.TryGetValue(id, out string? name) ? name : "";
    }
}
