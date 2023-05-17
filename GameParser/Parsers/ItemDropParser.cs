using System.Xml;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using SqlKata.Execution;

namespace GameParser.Parsers;

public static class ItemDropParser
{
    public static void Parse()
    {
        foreach (PackFileEntry? entry in Paths.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/individualitemdrop") && !entry.Name.StartsWith("table/na/individualitemdrop"))
            {
                continue;
            }

            XmlDocument? document = Paths.XmlReader.GetXmlDocument(entry);
            XmlNodeList individualBoxItems = document.SelectNodes("/ms2/individualDropBox")!;
            foreach (XmlNode node in individualBoxItems)
            {
                string locale = node.Attributes!["locale"]?.Value ?? "";

                if (locale != "NA" && locale != "")
                {
                    continue;
                }

                int boxId = int.Parse(node.Attributes["individualDropBoxID"]!.Value);
                float minAmount = float.Parse(node.Attributes["minCount"]!.Value);
                float maxAmount = float.Parse(node.Attributes["maxCount"]!.Value);
                int smartDropRate = int.Parse(node.Attributes["smartDropRate"]?.Value ?? "0");
                int groupDropId = int.Parse(node.Attributes["dropGroup"]!.Value);

                _ = byte.TryParse(node.Attributes["PackageUIShowGrade"]?.Value ?? "0", out byte rarity);
                if (rarity == 0)
                {
                    rarity = 1;
                }

                Console.WriteLine("Parsing box " + boxId);
                QueryManager.QueryFactory.Query("item_boxes").Insert(new
                {
                    box_id = boxId,
                    item_id = int.Parse(node.Attributes["item"]!.Value),
                    item_id2 = node.Attributes["item2"] is not null ? int.Parse(node.Attributes["item2"]!.Value) : 0,
                    min_count = minAmount,
                    max_count = maxAmount,
                    rarity,
                    smart_drop_rate = smartDropRate,
                    group_drop_id = groupDropId,
                });
            }
        }
    }
}