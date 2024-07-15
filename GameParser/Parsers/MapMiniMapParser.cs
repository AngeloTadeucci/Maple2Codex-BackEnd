using System.Xml;
using Maple2.File.IO.Crypto.Common;
using Maple2.File.Parser.Tools;
using Maple2Storage.Types;

namespace GameParser.Parsers;

public static class MapImagesParser {
    public static readonly Dictionary<string, (string minimap, string icon, string bg)> MapsImages = [];

    static MapImagesParser() {
        Filter.Load(Paths.XmlReader, "NA", "Live");
        foreach (PackFileEntry? item in Paths.XmlReader.Files.Where(x => x.Name.StartsWith("mapxblock/"))) {
            XmlDocument doc = Paths.XmlReader.GetXmlDocument(item);
            XmlNode? minimapNode = doc.SelectSingleNode("/ms2/minimap/image");
            if (minimapNode == null) {
                continue;
            }

            string name = item.Name.Split("/").Last().Split(".").First().ToLower();

            string minimap = minimapNode.Attributes?["name"]?.Value.Split("/").Last() ?? "";
            string icon = minimapNode.Attributes?["icon"]?.Value.Split("\\").Last().ToLower() ?? "";
            XmlNode? bgNode = doc.SelectSingleNode("/ms2/clientProperty");
            if (bgNode == null) {
                MapsImages[name] = (minimap, icon, "");
                continue;
            }
            string bg = bgNode.Attributes?["bgDay"]?.Value.ToLower() ?? "";
            if (bg.Contains(".swf")) {
                MapsImages[name] = (minimap, icon, "");
                return;
            }

            MapsImages[name] = (minimap.Replace(".dds", ".png"), icon.Replace(".dds", ".png"), bg.Replace(".dds", ".png"));
        }
    }
}
