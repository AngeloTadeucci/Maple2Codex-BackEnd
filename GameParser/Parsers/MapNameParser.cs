using System.Text.Json;
using System.Xml;
using Maple2.File.Parser.Tools;
using Maple2.File.Parser.Xml.Map;
using Maple2Storage.Types;
using SqlKata.Execution;

namespace GameParser.Parsers;
public static class MapNameParser {
    public static Dictionary<int, string> MapNames { get; private set; } = new Dictionary<int, string>();

    static MapNameParser() {
        Filter.Load(Paths.XmlReader, "NA", "Live");
        Maple2.File.Parser.MapParser parser = new(Paths.XmlReader);
        foreach ((int id, string? name, MapData _) in parser.Parse()) {
            MapNames[id] = name;
        }
    }

    public static void Parse() {
        JsonSerializerOptions options = new() {
            IncludeFields = true,
        };

        Filter.Load(Paths.XmlReader, "NA", "Live");
        Maple2.File.Parser.MapParser parser = new(Paths.XmlReader);
        foreach ((int id, string? name, MapData _) in parser.Parse()) {
            Console.WriteLine($"Parsing Map {id} - {name}");

            QueryManager.QueryFactory.Query("maps").Insert(new {
                id,
                name = string.IsNullOrEmpty(name) ? "" : name,
            });
        }
    }
}
