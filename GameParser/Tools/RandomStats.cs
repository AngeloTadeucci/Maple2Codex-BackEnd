using GameParser.Parsers;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameParser.Tools;

public static class RandomStats {
    public static void GetStats(Item item, out Dictionary<StatAttribute, StatRange> randomStats, out int slots) {
        randomStats = [];
        slots = 0;
        ItemOptionRandom? randomOptions = ItemOptionRandomParser.GetMetadata(item.RandomId, item.Rarity);
        if (randomOptions == null) {
            return;
        }

        Random random = Random.Shared;
        if (randomOptions.Slots.Length > 0) {
            slots = random.Next(randomOptions.Slots[0], randomOptions.Slots[1]);
        }

        IEnumerable<StatRange> itemStats = RollStats(randomOptions, item);

        foreach (StatRange? stat in itemStats) {
            randomStats[stat.ItemAttribute] = stat;
        }
    }

    private static List<StatRange> RollStats(ItemOptionRandom randomOptions, Item item) {
        List<StatRange> itemStats = [];

        Dictionary<StatAttribute, List<ParserStat>> rangeDictionary = GetRange(item);
        foreach (ParserStat? stat in randomOptions.Stats) {
            if (!rangeDictionary.ContainsKey(stat.Attribute)) {
                continue;
            }

            List<ParserStat> parserStats = rangeDictionary[stat.Attribute];
            ParserStat parserSpecialStatMin = parserStats[RollMinMax(item, true)];
            ParserStat parserSpecialStatMax = parserStats[RollMinMax(item, false)];
            StatRange normalStat = new() {
                ItemAttribute = parserSpecialStatMin.Attribute,
                AttributeType = parserSpecialStatMin.AttributeType,
                ValueMin = parserSpecialStatMin.Value,
                ValueMax = parserSpecialStatMax.Value,
            };

            if (randomOptions.MultiplyFactor > 0) {
                normalStat.ValueMin *= (int) Math.Ceiling(randomOptions.MultiplyFactor);
                normalStat.ValueMin *= randomOptions.MultiplyFactor;

                normalStat.ValueMax *= (int) Math.Ceiling(randomOptions.MultiplyFactor);
                normalStat.ValueMax *= randomOptions.MultiplyFactor;
            }

            itemStats.Add(normalStat);
        }

        Dictionary<StatAttribute, List<ParserSpecialStat>> specialRanges = GetSpecialRange(item);
        foreach (ParserSpecialStat? stat in randomOptions.SpecialStats) {
            if (!rangeDictionary.ContainsKey(stat.Attribute)) {
                continue;
            }

            List<ParserSpecialStat> parserSpecialStats = specialRanges[stat.Attribute];
            ParserSpecialStat parserSpecialStatMin = parserSpecialStats[RollMinMax(item, true)];
            ParserSpecialStat parserSpecialStatMax = parserSpecialStats[RollMinMax(item, false)];
            StatRange specialStat = new() {
                ItemAttribute = parserSpecialStatMin.Attribute,
                AttributeType = parserSpecialStatMin.AttributeType,
                ValueMin = parserSpecialStatMin.Value,
                ValueMax = parserSpecialStatMax.Value,
            };

            if (randomOptions.MultiplyFactor > 0) {
                specialStat.ValueMin *= (int) Math.Ceiling(randomOptions.MultiplyFactor);
                specialStat.ValueMin *= randomOptions.MultiplyFactor;

                specialStat.ValueMax *= (int) Math.Ceiling(randomOptions.MultiplyFactor);
                specialStat.ValueMax *= randomOptions.MultiplyFactor;
            }

            itemStats.Add(specialStat);
        }

        return itemStats;
    }

    private static Dictionary<StatAttribute, List<ParserStat>> GetRange(Item item) {
        if (Item.IsAccessory(item.Slot)) {
            return ItemOptionRangeParser.GetAccessoryRanges();
        }

        if (Item.IsArmor(item.Slot)) {
            return ItemOptionRangeParser.GetArmorRanges();
        }

        if (Item.IsWeapon(item.Slot)) {
            return ItemOptionRangeParser.GetWeaponRanges();
        }

        return ItemOptionRangeParser.GetPetRanges();
    }

    private static Dictionary<StatAttribute, List<ParserSpecialStat>> GetSpecialRange(Item item) {
        if (Item.IsAccessory(item.Slot)) {
            return ItemOptionRangeParser.GetAccessorySpecialRanges();
        }

        if (Item.IsArmor(item.Slot)) {
            return ItemOptionRangeParser.GetArmorSpecialRanges();
        }

        if (Item.IsWeapon(item.Slot)) {
            return ItemOptionRangeParser.GetWeaponSpecialRanges();
        }

        return ItemOptionRangeParser.GetPetSpecialRanges();
    }

    private static int RollMinMax(Item item, bool minRoll) {
        if (item.OptionLevelFactor >= 70) {
            return minRoll ? 8 : 15;
        }

        return minRoll ? 0 : 7;
    }
}
