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
        Filter.Load(Paths.XmlReader, "NA", "Live");
        Maple2.File.Parser.MapParser parser = new(Paths.XmlReader);
        foreach ((int id, string? name, MapData data) in parser.Parse()) {
            Console.WriteLine($"Parsing Map {id} - {name}");
            string xblock = data.xblock.name.ToLower();
            MapImagesParser.MapsImages.TryGetValue(xblock, out (string minimap, string icon, string bg) images);

            QueryManager.QueryFactory.Query("maps").Insert(new {
                id,
                name = string.IsNullOrEmpty(name) ? "" : name,
                minimap = images.minimap ?? "",
                icon = images.icon ?? "",
                bg = images.bg ?? ""
            });
        }
    }
}
