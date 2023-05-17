using GameParser.Parsers;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MoonSharp.Interpreter;

namespace GameParser.Tools;

public static class ConstantStats
{
    public static void GetStats(Item item, out Dictionary<StatAttribute, Stat> constantStats)
    {
        constantStats = new();
        int constantId = item.OptionConstantId;
        ItemOptionsConstant? basicOptions = ItemOptionConstantParser.GetMetadata(constantId, item.Rarity);
        if (basicOptions == null)
        {
            GetDefault(item, constantStats);
            return;
        }

        foreach (ParserStat? stat in basicOptions.Stats)
        {
            constantStats[stat.Attribute] = new()
            {
                ItemAttribute = stat.Attribute,
                AttributeType = stat.AttributeType,
                Value = stat.Value,
            };
        }

        foreach (ParserSpecialStat? stat in basicOptions.SpecialStats)
        {
            constantStats[stat.Attribute] = new()
            {
                ItemAttribute = stat.Attribute,
                AttributeType = stat.AttributeType,
                Value = stat.Value,
            };
        }

        // TODO: Implement Hidden ndd (defense) and wapmax (Max Weapon Attack)

        if (item.OptionLevelFactor > 50)
        {
            GetDefault(item, constantStats);
        }
    }

    private static void GetDefault(Item item, Dictionary<StatAttribute, Stat> constantStats)
    {
        ItemOptionPick? baseOptions = ItemOptionPickParser.GetMetadata(item.OptionId, item.Rarity);
        if (baseOptions is null)
        {
            return;
        }

        Script? script = ScriptLoader.GetScript("Functions/calcItemValues");

        foreach (ConstantPick? constantPick in baseOptions.Constants)
        {
            string calcScript;
            switch (constantPick.Stat)
            {
                case StatAttribute.Hp:
                    calcScript = "constant_value_hp";
                    break;
                case StatAttribute.Defense:
                    calcScript = "constant_value_ndd";
                    break;
                case StatAttribute.MagicRes:
                    calcScript = "constant_value_mar";
                    break;
                case StatAttribute.PhysicalRes:
                    calcScript = "constant_value_par";
                    break;
                case StatAttribute.CritRate:
                    calcScript = "constant_value_cap";
                    break;
                case StatAttribute.Str:
                    calcScript = "constant_value_str";
                    break;
                case StatAttribute.Dex:
                    calcScript = "constant_value_dex";
                    break;
                case StatAttribute.Int:
                    calcScript = "constant_value_int";
                    break;
                case StatAttribute.Luk:
                    calcScript = "constant_value_luk";
                    break;
                case StatAttribute.MagicAtk:
                    calcScript = "constant_value_map";
                    break;
                case StatAttribute.MinWeaponAtk:
                    calcScript = "constant_value_wapmin";
                    break;
                case StatAttribute.MaxWeaponAtk:
                    calcScript = "constant_value_wapmax";
                    break;
                default:
                    continue;
            }

            constantStats.TryAdd(constantPick.Stat, new()
            {
                ItemAttribute = constantPick.Stat,
                AttributeType = StatAttributeType.Flat,
                Value = 0,
            });

            float statValue = constantStats[constantPick.Stat].Value;
            DynValue? result = script?.RunFunction(calcScript, statValue, constantPick.DeviationValue, (int) item.Type,
                item.RecommendJobs.First(), item.OptionLevelFactor, item.Rarity, item.Level);
            if (result is null)
            {
                return;
            }

            constantStats[constantPick.Stat].Value = (float) result.Number;
            if (constantStats[constantPick.Stat].Value <= 0.0000f)
            {
                constantStats.Remove(constantPick.Stat);
            }
        }
    }
}