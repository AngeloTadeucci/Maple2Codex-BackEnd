using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Xml;
using GameParser.DescriptionHelper;
using GameParser.Tools;
using Maple2.File.IO.Crypto.Common;
using Maple2.File.Parser.Tools;
using Maple2.File.Parser.Xml.Item;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MoonSharp.Interpreter;
using SqlKata.Execution;

namespace GameParser.Parsers;

public static class ItemParser {
    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
    public static void Parse() {
        // Item rarity
        Dictionary<int, int> rarities = ParseItemRarities();

        // descriptions
        Dictionary<int, (string tooltip, string guide, string main)> descriptions = ParseItemDescriptions();

        Filter.Load(Paths.XmlReader, "NA", "Live");
        Maple2.File.Parser.ItemParser parser = new(Paths.XmlReader);

        Dictionary<int, string> itemNames = [];
        foreach ((int id, string? name, ItemData _) in parser.Parse()) {
            itemNames[id] = name;
        }

        foreach ((int id, string? name, ItemData? data) in parser.Parse()) {
            Console.WriteLine($"Parsing item {id} - {name}");
            Limit? limit = data.limit;
            Skill? skill = data.skill;
            Fusion? fusion = data.fusion;
            Property? property = data.property;
            Function? function = data.function;
            Install? install = data.install;
            MusicScore? musicScore = data.MusicScore;
            Life? life = data.life;
            Housing? housing = data.housing;
            AdditionalEffect? additionalEffect = data.AdditionalEffect;
            Slots? slots = data.slots;
            Customize? customize = data.customize;

            List<string> kfms = [];
            if (slots is not null) {
                foreach (Slot? slot in slots.slot) {
                    foreach (Slot.Asset? asset in slot.asset) {
                        string kmfName = asset.name.Contains("urn")
                            ? asset.name.Split(":").Last()
                            : asset.name.Split('/').Last().Split(".nif").First().ToLower();

                        kfms.Add($"{kmfName.ToLower()}");
                    }
                }
            }

            (string tooltip, string guide, string main) = descriptions.GetValueOrDefault(id);

            List<int> jobLimit = limit.jobLimit.Length == 0 ? [0] : [.. limit.jobLimit];

            List<int> recommendJobs = limit.recommendJobs.Length == 0 ? [0] : [.. limit.recommendJobs];

            int rarity = GetRarity(id, data.option.constant, data.option.random, data.option.@static, rarities);
            bool _ = Enum.TryParse(data.property.category, out ItemSlot itemSlot);

            Item item = new(id, rarity, jobLimit, GetItemType(id), limit.levelLimit, data.option.constant,
                data.option.@static, data.option.optionID, (int) data.option.optionLevelFactor, data.option.random, itemSlot, property.gearScore);
            StatsParser.ParseStats(item,
                out List<(Stat stat, string description)> constantStats,
                out List<(StatRange stat, string description)> staticStats,
                out List<(StatRange stat, string description)> randomStats,
                out int randomStatCount);

            JsonSerializerOptions options = new() {
                IncludeFields = true,
            };
            SetItemInfoMetadata? setInfo = SetItemInfoParser.GetMetadata(id);
            List<(string itemNames, int id)> setData = [];
            if (setInfo is not null) {
                foreach (int itemId in setInfo.ItemIds) {
                    setData.Add((itemNames[itemId], itemId));
                }
            }

            string setName = SetItemNameParser.GetSetItemName(setInfo?.Id ?? 0);
            string preset = data.tool.itemPresetPath.Contains('/') ? data.tool.itemPresetPath.Split('/')[1] : "";

            int boxId = 0;
            switch (function.name) {
                // selection boxes are SelectItemBox and 1,boxid
                // normal boxes are OpenItemBox and 0,1,0,boxid
                // fragments are OpenItemBox and 0,1,0,boxid,required_amount
                case "SelectItemBox" when function.parameter.Contains('l'):
                case "OpenItemBox" when function.parameter.Contains('l'):
                    break; // TODO: Implement these CN items. Skipping for now
                case "OpenItemBox": {
                        List<string> parameters = new(function.parameter.Split(','));
                        boxId = int.Parse(parameters[3]);
                        break;
                    }
                case "SelectItemBox": {
                        List<string> parameters = new(function.parameter.Split(','));
                        parameters.RemoveAll(param => param.Length == 0);
                        boxId = int.Parse(parameters[1]);
                        break;
                    }
            }

            QueryManager.QueryFactory.Query("items").Insert(new {
                id,
                name = string.IsNullOrEmpty(name) ? string.Empty : name,
                tooltip_description = string.IsNullOrEmpty(tooltip) ? string.Empty : tooltip,
                guide_description = string.IsNullOrEmpty(guide) ? string.Empty : guide,
                main_description = string.IsNullOrEmpty(main) ? string.Empty : main,
                rarity,
                is_outfit = property.skin,
                job_limit = JsonSerializer.Serialize(jobLimit),
                job_recommend = JsonSerializer.Serialize(recommendJobs),
                level_min = limit.levelLimit,
                level_max = limit.levelLimitMax,
                gender = limit.genderLimit,
                icon_path =
                    property.slotIcon.Equals("icon0.png", StringComparison.CurrentCultureIgnoreCase)
                        ? property.slotIconCustom.ToLower()
                        : property.slotIcon.ToLower(),
                pet_id = data.pet?.petID ?? 0,
                is_ugc = !string.IsNullOrEmpty(data.ucc.mesh),
                transfer_type = limit.transferType,
                sellable = limit.shopSell,
                breakable = limit.enableBreak,
                fusionable = fusion.fusionable == 1,
                skill_id = skill.skillID,
                skill_level = skill.skillLevel,
                stack_limit = property.slotMax,
                tradeable_count = property.tradableCount,
                repackage_limit = property.rePackingLimitCount,
                repackage_scrolls = string.Join(",", property.globalRePackingScrollID ?? Array.Empty<int>()),
                repackage_count = property.globalRePackingItemConsumeCount ?? 0,
                sell_price = JsonSerializer.Serialize(property.sell.price.ToList()),
                kfms = JsonSerializer.Serialize(kfms),
                icon_code = property.iconCode,
                move_disable = property.moveDisable == 1,
                remake_disable = property.remakeDisable,
                enchantable = limit.exceptEnchant,
                dyeable = customize?.color == 1,
                constants_stats = JsonSerializer.Serialize(constantStats, options),
                static_stats = JsonSerializer.Serialize(staticStats, options),
                random_stats = JsonSerializer.Serialize(randomStats, options),
                random_stat_count = randomStatCount,
                gear_score = GetGearScore(item),
                slot = itemSlot,
                set_info = JsonSerializer.Serialize(setData, options),
                set_name = setName,
                item_preset = preset,
                glamour_count = ItemExtractionParser.GetMetadata(id)?.TryCount ?? 0,
                box_id = boxId,
                item_type = property.type,
                represent_option = property.representOption,
            });
        }
    }

    public static int GetRarity(int id, int optionConstant, int optionRandom, int optionStatic, Dictionary<int, int> rarities) {
        // webfinder
        if (rarities.TryGetValue(id, out int rarity)) {
            return rarity;
        }

        // constants
        ItemOptionConstantMetadata? basicOptions = ItemOptionConstantParser.GetMetadata(optionConstant);
        if (basicOptions is not null) {
            return basicOptions.ItemOptions.Count > 1 ? basicOptions.ItemOptions.Max(x => x.Rarity) : basicOptions.ItemOptions[0].Rarity;
        }

        // random
        ItemOptionRandomMetadata? randomOptions = ItemOptionRandomParser.GetMetadata(optionRandom);
        if (randomOptions is not null) {
            return randomOptions.ItemOptions.Count > 1 ? randomOptions.ItemOptions.Max(x => x.Rarity) : randomOptions.ItemOptions[0].Rarity;
        }

        // static
        ItemOptionStaticMetadata? staticOptions = ItemOptionStaticParser.GetMetadata(optionStatic);
        if (staticOptions is not null) {
            return staticOptions.ItemOptions.Count > 1 ? staticOptions.ItemOptions.Max(x => x.Rarity) : staticOptions.ItemOptions[0].Rarity;
        }

        // no options, default to 1
        return 1;
    }

    public static ItemType GetItemType(int id) {
        //TODO: Find a better method to find the item type
        return (id / 100000) switch {
            112 => ItemType.Earring,
            113 => ItemType.Hat,
            114 => ItemType.Clothes,
            115 => ItemType.Pants,
            116 => ItemType.Gloves,
            117 => ItemType.Shoes,
            118 => ItemType.Cape,
            119 => ItemType.Necklace,
            120 => ItemType.Ring,
            121 => ItemType.Belt,
            122 => ItemType.Overall,
            130 => ItemType.Bludgeon,
            131 => ItemType.Dagger,
            132 => ItemType.Longsword,
            133 => ItemType.Scepter,
            134 => ItemType.ThrowingStar,
            140 => ItemType.Spellbook,
            141 => ItemType.Shield,
            150 => ItemType.Greatsword,
            151 => ItemType.Bow,
            152 => ItemType.Staff,
            153 => ItemType.Cannon,
            154 => ItemType.Blade,
            155 => ItemType.Knuckle,
            156 => ItemType.Orb,
            209 => ItemType.Medal,
            410 or 420 or 430 => ItemType.Lapenshard,
            501 or 502 or 503 or 504 or 505 => ItemType.Furnishing,
            600 => ItemType.Pet,
            900 => ItemType.Currency,
            _ => ItemType.None
        };
    }

    public static int GetGearScore(Item item) {
        Script? script = ScriptLoader.GetScript("Functions/calcItemValues");
        DynValue? result = script?.RunFunction("calcItemGearScore", item.GearScoreValue, item.Rarity, (int) item.Type, 0, 0);

        if (result is null) {
            return 0;
        }

        return (int) result.Tuple[0].Number + (int) result.Tuple[1].Number;
    }

    private static Dictionary<int, (string tooltip, string guide, string main)> ParseItemDescriptions() {
        Dictionary<int, (string tooltip, string guide, string main)> descriptions = [];
        XmlDocument? xmlFile =
            Paths.XmlReader.GetXmlDocument(Paths.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("string/en/koritemdescription.xml")));

        if (xmlFile is null) {
            throw new("Failed to load koritemdescription.xml");
        }

        XmlNodeList? nodes = xmlFile.SelectNodes("/ms2/key");
        if (nodes is null) {
            throw new("Failed to load koritemdescription.xml");
        }
        foreach (XmlNode node in nodes) {
            int id = int.Parse(node.Attributes?["id"]?.Value ?? "0");
            if (id == 0) {
                continue;
            }

            if (descriptions.ContainsKey(id)) {
                continue;
            }

            string tooltip = Helper.FixDescription(node.Attributes?["tooltipDescription"]?.Value ?? "");
            string guide = Helper.FixDescription(node.Attributes?["guideDescription"]?.Value ?? "");
            string main = Helper.FixDescription(node.Attributes?["mainDescription"]?.Value ?? "");
            descriptions[id] = (tooltip, guide, main);
        }

        return descriptions;
    }

    private static Dictionary<int, int> ParseItemRarities() {
        Dictionary<int, int> rarities = [];
        foreach (PackFileEntry? entry in Paths.XmlReader.Files) {
            if (!entry.Name.StartsWith("table/na/itemwebfinder")) {
                continue;
            }

            XmlDocument? innerDocument = Paths.XmlReader.GetXmlDocument(entry);
            XmlNodeList? nodes = innerDocument.SelectNodes("/ms2/key");
            if (nodes is null) {
                continue;
            }
            foreach (XmlNode node in nodes) {
                int itemId = int.Parse(node.Attributes!["id"]!.Value);
                int rarity = int.Parse(node.Attributes["grade"]!.Value);
                rarities[itemId] = rarity;
            }
        }

        return rarities;
    }

    private static Dictionary<int, List<ItemBreakReward>> ParseItemBreakingIngredients() {
        Dictionary<int, List<ItemBreakReward>> rewards = [];
        foreach (PackFileEntry? entry in Paths.XmlReader.Files) {
            if (!entry.Name.StartsWith("table/itembreakingredient")) {
                continue;
            }

            XmlDocument? innerDocument = Paths.XmlReader.GetXmlDocument(entry);
            XmlNodeList? individualItems = innerDocument.SelectNodes("/ms2/item");
            if (individualItems is null) {
                continue;
            }
            foreach (XmlNode nodes in individualItems) {
                string locale = nodes.Attributes?["locale"]?.Value ?? "";
                if (locale != "NA" && locale != "") {
                    continue;
                }

                int itemId = int.Parse(nodes.Attributes!["ItemID"]!.Value);
                rewards[itemId] = [];

                int ingredientItemId1 = int.Parse(nodes.Attributes["IngredientItemID1"]?.Value ?? "0");
                int ingredientCount1 = int.Parse(nodes.Attributes["IngredientCount1"]?.Value ?? "0");
                rewards[itemId].Add(new(ingredientItemId1, ingredientCount1));

                _ = int.TryParse(nodes.Attributes["IngredientItemID2"]?.Value ?? "0", out int ingredientItemId2);
                _ = int.TryParse(nodes.Attributes["IngredientCount2"]?.Value ?? "0", out int ingredientCount2);
                rewards[itemId].Add(new(ingredientItemId2, ingredientCount2));

                _ = int.TryParse(nodes.Attributes["IngredientItemID3"]?.Value ?? "0", out int ingredientItemId3);
                _ = int.TryParse(nodes.Attributes["IngredientCount3"]?.Value ?? "0", out int ingredientCount3);
                rewards[itemId].Add(new(ingredientItemId3, ingredientCount3));
            }
        }

        return rewards;
    }

    private static InventoryTab GetTab(int type, int subType, bool skin = false, bool survival = false) {
        if (skin) {
            return InventoryTab.Outfit;
        }

        if (survival) {
            //return InventoryTab.Survival;
        }

        switch (type) {
            case 0: // Unknown
                return InventoryTab.Misc;
            case 1:
                return InventoryTab.Gear;
            case 2: // "Usable"
                switch (subType) {
                    case 2:
                        return InventoryTab.Consumable;
                    case 8: // Skill book for mount?
                        return InventoryTab.Mount;
                    case 14: // Emote
                        return InventoryTab.Misc;
                }

                break;
            case 3:
                return InventoryTab.Quest;
            case 4:
                return InventoryTab.Misc;
            case 5: // Air mount
                return InventoryTab.Mount;
            case 6: // Furnishing shows up in FishingMusic
                return InventoryTab.FishingMusic;
            case 7:
                return InventoryTab.Badge;
            case 9: // Ground mount
                return InventoryTab.Mount;
            case 10:
                switch (subType) {
                    case 0:
                    case 4:
                    case 5: // Ad Balloon
                    case 11: // Survival Medals
                    case 15: // Voucher
                    case 17: // Packages
                    case 18: // Packages
                    case 19:
                        return InventoryTab.Misc;
                    case 20: // Fishing Pole / Instrument
                        return InventoryTab.FishingMusic;
                }

                break;
            case 11:
                return InventoryTab.Pets;
            case 12: // Music Score
                return InventoryTab.FishingMusic;
            case 13:
            case 14: // Gem dust
                return InventoryTab.Gemstone;
            case 15:
                return InventoryTab.Catalyst;
            case 16:
                return InventoryTab.LifeSkill;
            case 19:
                return InventoryTab.Misc;
            case 20:
                return InventoryTab.Currency;
            case 21:
                return InventoryTab.Lapenshard;
            case 22: // Blueprint
                return InventoryTab.Misc;
        }

        throw new ArgumentException($"Unknown Tab for: {type},{subType}");
    }
}
