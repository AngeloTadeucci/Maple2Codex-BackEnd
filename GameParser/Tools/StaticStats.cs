using GameParser.Parsers;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MoonSharp.Interpreter;

namespace GameParser.Tools;

public static class StaticStats {
    public static void GetStats(Item item, out Dictionary<StatAttribute, StatRange> staticStats) {
        staticStats = [];
        if (item.OptionLevelFactor < 50) {
            return;
        }

        int staticId = item.OptionStaticId;

        ItemOptionsStatic? staticOptions = ItemOptionStaticParser.GetMetadata(staticId, item.Rarity);
        if (staticOptions == null) {
            GetDefault(item, staticStats);
            return;
        }

        foreach (ParserStat? stat in staticOptions.Stats) {
            staticStats[stat.Attribute] = new() {
                ItemAttribute = stat.Attribute,
                AttributeType = stat.AttributeType,
                ValueMin = stat.Value,
                ValueMax = stat.Value,
            };
        }

        foreach (ParserSpecialStat? stat in staticOptions.SpecialStats) {
            staticStats[stat.Attribute] = new() {
                ItemAttribute = stat.Attribute,
                AttributeType = stat.AttributeType,
                ValueMin = stat.Value,
                ValueMax = stat.Value,
            };
        }

        // TODO: Implement Hidden ndd (defense) and wapmax (Max Weapon Attack)

        GetDefault(item, staticStats);
    }

    private static void GetDefault(Item item, Dictionary<StatAttribute, StatRange> stats) {
        ItemOptionPick? baseOptions = ItemOptionPickParser.GetMetadata(item.OptionId, item.Rarity);
        if (baseOptions is null) {
            return;
        }

        Script? script = ScriptLoader.GetScript("Functions/calcItemValues");
        if (script is null) {
            return;
        }
        foreach (StaticPick? staticPickFlat in baseOptions.StaticValues) {
            SetStat(stats, staticPickFlat, item, script, item.OptionLevelFactor);
        }

        foreach (StaticPick? staticPickRate in baseOptions.StaticRates) {
            SetStat(stats, staticPickRate, item, script, item.OptionLevelFactor);
        }
    }

    private static void SetStat(Dictionary<StatAttribute, StatRange> stats,
        StaticPick staticPick, Item item, Script script, float optionLevelFactor) {
        stats.TryAdd(staticPick.Stat, new() {
            ItemAttribute = staticPick.Stat,
            AttributeType = StatAttributeType.Flat,
            ValueMin = 0,
            ValueMax = 0,
        });

        StatRange valueTuple = stats[staticPick.Stat];
        float currentStatValue = stats[staticPick.Stat].ValueMin;

        (double min, double max) statValue = CalculateStat(item, optionLevelFactor, staticPick, script, currentStatValue);

        stats[staticPick.Stat].ValueMin = (float) statValue.min;
        stats[staticPick.Stat].ValueMax = (float) statValue.max;

        if (stats[staticPick.Stat].ValueMin <= 0.0000f || stats[staticPick.Stat].ValueMax <= 0.0000f) {
            stats.Remove(staticPick.Stat);
        } else {
            stats[staticPick.Stat] = valueTuple;
        }
    }

    private static (double min, double max) CalculateStat(Item item, float optionLevelFactor, StaticPick staticPick, Script script, float currentStatValue) {
        string calcScript;
        switch (staticPick.Stat) {
            case StatAttribute.Hp:
                calcScript = "static_value_hp";
                break;
            case StatAttribute.Defense: // TODO: this is not calculating correctly
                calcScript = "static_value_ndd";
                break;
            case StatAttribute.MagicRes:
                calcScript = "static_value_mar";
                break;
            case StatAttribute.PhysicalRes:
                calcScript = "static_value_par";
                break;
            case StatAttribute.PhysicalAtk:
                calcScript = "static_value_pap";
                break;
            case StatAttribute.MagicAtk:
                calcScript = "static_value_map";
                break;
            case StatAttribute.MaxWeaponAtk:
                calcScript = "static_value_wapmax";
                break;
            case StatAttribute.PerfectGuard:
                calcScript = "static_rate_abp";
                break;
            default:
                return (0, 0);
        }

        DynValue? result = script.RunFunction(calcScript, currentStatValue, staticPick.DeviationValue, (int) item.Type,
            item.RecommendJobs.First(), optionLevelFactor, item.Rarity, item.Level);

        if (result is null) {
            return (0, 0);
        }
        if (result.Tuple.Length < 2) {
            return (0, 0);
        }

        if (result.Tuple[0].Number == 0 && result.Tuple[1].Number == 0) {
            return (0, 0);
        }


        return (result.Tuple[0].Number, result.Tuple[1].Number);
    }
}
