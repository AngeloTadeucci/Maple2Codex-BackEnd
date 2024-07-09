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

    private static IEnumerable<StatRange> RollStats(ItemOptionRandom randomOptions, Item item) {
        List<StatRange> itemStats = [];

        foreach (ParserStat? stat in randomOptions.Stats) {
            Dictionary<StatAttribute, List<ParserStat>> rangeDictionary = GetRange(item);
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

        foreach (ParserSpecialStat? stat in randomOptions.SpecialStats) {
            Dictionary<StatAttribute, List<ParserSpecialStat>> rangeDictionary = GetSpecialRange(item);
            if (!rangeDictionary.ContainsKey(stat.Attribute)) {
                continue;
            }

            List<ParserSpecialStat> parserSpecialStats = rangeDictionary[stat.Attribute];
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

    // Returns index 0~7 for equip level 70-
    // Returns index 8~15 for equip level 70+
    private static int Roll(Item item) {
        Random random = Random.Shared;
        if (item.OptionLevelFactor >= 70) {
            return random.NextDouble() switch {
                >= 0.0 and < 0.24 => 8,
                >= 0.24 and < 0.48 => 9,
                >= 0.48 and < 0.74 => 10,
                >= 0.74 and < 0.9 => 11,
                >= 0.9 and < 0.966 => 12,
                >= 0.966 and < 0.985 => 13,
                >= 0.985 and < 0.9975 => 14,
                _ => 15
            };
        }

        return random.NextDouble() switch {
            >= 0.0 and < 0.24 => 0,
            >= 0.24 and < 0.48 => 1,
            >= 0.48 and < 0.74 => 2,
            >= 0.74 and < 0.9 => 3,
            >= 0.9 and < 0.966 => 4,
            >= 0.966 and < 0.985 => 5,
            >= 0.985 and < 0.9975 => 6,
            _ => 7
        };
    }

    private static int RollMinMax(Item item, bool minRoll) {
        if (item.OptionLevelFactor >= 70) {
            return minRoll ? 8 : 15;
        }

        return minRoll ? 0 : 7;
    }
}
