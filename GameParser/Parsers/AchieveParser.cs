using System.Xml;
using Maple2Storage.Types;
using SqlKata.Execution;
using GameParser.DescriptionHelper;
using Maple2.File.Parser.Xml.Achieve;
using Maple2.File.Parser.Tools;

namespace GameParser.Parsers;
public static class AchieveParser {
    public static Dictionary<int, string> AchieveNames { get; private set; } = new Dictionary<int, string>();

    public static void Parse() {

        Dictionary<int, (string description, string complete_description)> descriptions = ParseAchieveDescriptions();

        Filter.Load(Paths.XmlReader, "NA", "Live");
        Maple2.File.Parser.AchieveParser parser = new(Paths.XmlReader);
        foreach ((int id, string? name, AchieveData data) in parser.Parse()) {
            AchieveNames[id] = name;

            Console.WriteLine($"Achieve {id} - {name}");

            string description = "";
            if (descriptions.ContainsKey(id)) {
                description = descriptions[id].description;
            }

            string complete_description = "";
            if (descriptions.ContainsKey(id)) {
                complete_description = descriptions[id].description;
            }

            string fixedName = "";
            if (name is not null) {
                fixedName = Helper.FixDescription(name);
            }

            QueryManager.QueryFactory.Query("achieves").Insert(new {
                id,
                name = fixedName,
                description,
                complete_description,
                icon = data.icon.ToLower(),
            });
        }
    }

    private static Dictionary<int, (string description, string complete_description)> ParseAchieveDescriptions() {
        Dictionary<int, (string description, string complete_description)> descriptions = [];
        XmlDocument? xmlFile =
            Paths.XmlReader.GetXmlDocument(Paths.XmlReader.Files.First(x => x.Name.StartsWith("string/en/achievedescription.xml")));

        if (xmlFile is null) {
            throw new("Failed to load achievedescription.xml");
        }

        XmlNodeList? nodes = xmlFile.SelectNodes("/ms2/achieve");
        if (nodes is null) {
            throw new("Failed to load achievedescription.xml");
        }
        foreach (XmlNode node in nodes) {
            int id = int.Parse(node.Attributes?["id"]?.Value ?? "0");
            if (id == 0) {
                continue;
            }

            if (descriptions.ContainsKey(id)) {
                continue;
            }

            string description = Helper.FixDescription(node.Attributes?["desc"]?.Value ?? "");
            string complete_description = Helper.FixDescription(node.Attributes?["complete"]?.Value ?? "");

            if (string.IsNullOrEmpty(description)) {
                description = Helper.FixDescription(node.Attributes?["manualDesc"]?.Value ?? "");
                complete_description = Helper.FixDescription(node.Attributes?["manualComplete"]?.Value ?? "");
            }


            descriptions[id] = (description, complete_description);
        }

        return descriptions;
    }
}
