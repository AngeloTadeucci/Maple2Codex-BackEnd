using Maple2Storage.Enums;

namespace Maple2Storage.Types;

public class Item {
    public Item(int id, int rarity, List<int> recommendJobs, ItemType type, int level, int optionConstantId, int optionStaticId, int optionId,
        int optionLevelFactor, int randomId, ItemSlot slot, int gearScoreValue) {
        Id = id;
        Rarity = rarity;
        RecommendJobs = recommendJobs;
        Type = type;
        Level = level;
        OptionConstantId = optionConstantId;
        OptionStaticId = optionStaticId;
        OptionId = optionId;
        OptionLevelFactor = optionLevelFactor;
        RandomId = randomId;
        Slot = slot;
        GearScoreValue = gearScoreValue;
    }

    public int Id { get; init; }
    public int Rarity { get; set; }
    public List<int> RecommendJobs { get; init; }
    public ItemType Type { get; init; }
    public int Level { get; init; }
    public int OptionConstantId { get; init; }
    public int OptionStaticId { get; init; }
    public int OptionId { get; init; }
    public int OptionLevelFactor { get; init; }
    public int RandomId { get; init; }
    public ItemSlot Slot { get; init; }
    public int GearScoreValue { get; init; }

    public static bool IsWeapon(ItemSlot slot) {
        return slot is ItemSlot.RH or ItemSlot.LH or ItemSlot.OH;
    }

    public static bool IsAccessory(ItemSlot slot) {
        return slot is ItemSlot.FH or ItemSlot.EA or ItemSlot.PD or ItemSlot.BE or ItemSlot.RI;
    }

    public static bool IsArmor(ItemSlot slot) {
        return slot is ItemSlot.CP or ItemSlot.CL or ItemSlot.GL or ItemSlot.SH or ItemSlot.MT;
    }
}

public class Stat {
    public StatAttribute ItemAttribute { get; set; }
    public StatAttributeType AttributeType { get; set; }
    public float Value { get; set; }
}

public class StatRange {
    public StatAttribute ItemAttribute { get; set; }
    public StatAttributeType AttributeType { get; set; }
    public float ValueMin { get; set; }
    public float ValueMax { get; set; }
}
