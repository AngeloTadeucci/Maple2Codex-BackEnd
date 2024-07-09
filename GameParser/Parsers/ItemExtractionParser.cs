using System.Xml;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameParser.Parsers;

public static class ItemExtractionParser {
    private static readonly Dictionary<int, ItemExtractionMetadata> ItemExtractionMetadatas = [];

    static ItemExtractionParser() {
        foreach (PackFileEntry? entry in Paths.XmlReader.Files) {
            if (!entry.Name.StartsWith("table/na/itemextraction")) {
                continue;
            }

            XmlDocument? document = Paths.XmlReader.GetXmlDocument(entry);
            XmlNodeList? nodes = document.SelectNodes("/ms2/key");
            if (nodes is null) {
                throw new("Failed to load itemextraction.xml");
            }

            foreach (XmlNode node in nodes) {
                if (node?.Attributes is null) {
                    continue;
                }

                ItemExtractionMetadata metadata = new() {
                    SourceItemId = int.Parse(node.Attributes["TargetItemID"]?.Value ??
                                             throw new("Failed to load itemextraction.xml")),
                    TryCount = byte.Parse(node.Attributes["TryCount"]?.Value ??
                                          throw new("Failed to load itemextraction.xml")),
                    ScrollCount = byte.Parse(node.Attributes["ScrollCount"]?.Value ??
                                             throw new("Failed to load itemextraction.xml")),
                    ResultItemId = int.Parse(node.Attributes["ResultItemID"]?.Value ??
                                             throw new("Failed to load itemextraction.xml"))
                };

                ItemExtractionMetadatas.Add(metadata.SourceItemId, metadata);
            }
        }
    }

    public static ItemExtractionMetadata? GetMetadata(int itemId) {
        return ItemExtractionMetadatas.GetValueOrDefault(itemId);
    }
}
