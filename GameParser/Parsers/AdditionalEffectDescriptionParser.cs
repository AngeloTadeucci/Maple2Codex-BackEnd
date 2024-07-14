using System.Xml;
using GameParser.DescriptionHelper;
using Maple2Storage.Types;

namespace GameParser.Parsers;

public static class AdditionalEffectDescriptionParser {
    public static readonly Dictionary<int, List<(int level, string name, string tooltipDescription)>> additionalEffectNames = [];

    static AdditionalEffectDescriptionParser() {
        Maple2.File.IO.Crypto.Common.PackFileEntry[] files = Paths.XmlReader.Files
            .Where(x => x.Name.StartsWith("string/en/koradditionaldescription"))
            .ToArray();
        foreach (Maple2.File.IO.Crypto.Common.PackFileEntry file in files) {
            XmlDocument? xmlFile = Paths.XmlReader.GetXmlDocument(file) ?? throw new("Failed to load " + file.Name);

            XmlNodeList? nodes = xmlFile.SelectNodes("/ms2/key") ?? throw new($"Failed to load {file.Name}");

            foreach (XmlNode node in nodes) {
                int id = int.Parse(node.Attributes?["id"]?.Value ?? throw new($"Failed to load {file.Name}"));
                string name = node.Attributes["name"]?.Value ?? "";

                int level = int.Parse(node.Attributes["level"]?.Value ?? "0");
                string tooltipDescription = node.Attributes["tooltipDescription"]?.Value ?? "";

                if (!additionalEffectNames.TryGetValue(id, out List<(int level, string name, string tooltipDescription)>? list)) {
                    list = [];

                    additionalEffectNames[id] = list;
                }

                list.Add((level, name, Helper.FixDescription(tooltipDescription)));
            }
        }
    }
}
