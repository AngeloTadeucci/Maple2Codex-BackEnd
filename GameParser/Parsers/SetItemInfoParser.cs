using System.Xml;
using GameParser.Tools;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameParser.Parsers;

public static class SetItemInfoParser
{
    private static readonly Dictionary<int, SetItemInfoMetadata> SetItemInfo = new();
    
    static SetItemInfoParser()
    {
        foreach (PackFileEntry? entry in Paths.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/setiteminfo"))
            {
                continue;
            }

            XmlDocument? innerDocument = Paths.XmlReader.GetXmlDocument(entry);
            XmlNodeList? nodeList = innerDocument.SelectNodes("/ms2/set");
            if (nodeList is null)
            {
                throw new("Failed to load setiteminfo.xml");
            }
            foreach (XmlNode node in nodeList)
            {
                int id = int.Parse(node.Attributes!["id"]?.Value ?? throw new("Failed to load setiteminfo.xml"));
                int[] itemIds = node.Attributes["itemIDs"]?.Value.SplitAndParseToInt(',').ToArray() ?? Array.Empty<int>();
                int optionId = int.Parse(node.Attributes["optionID"]?.Value ?? "0");

                SetItemInfo[id] = new()
                {
                    Id = id,
                    ItemIds = itemIds,
                    OptionId = optionId,
                };
            }
        }
    }
    
    public static SetItemInfoMetadata? GetMetadata(int itemId)
    {
        return SetItemInfo.Values.FirstOrDefault(x => x.ItemIds.Contains(itemId));
    }
}