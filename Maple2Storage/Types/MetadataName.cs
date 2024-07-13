﻿namespace Maple2Storage.Types;

public static class MetadataName {
    public const string AdBanner = "ad-banner";
    public const string AdditionalEffect = "additional-effect";
    public const string Animation = "animation";
    public const string BlackMarketTable = "black-market-table";
    public const string CharacterCreate = "character-create";
    public const string ChatSticker = "chat-sticker";
    public const string ColorPalette = "color-palette";
    public const string Constants = "constants";
    public const string DefaultItems = "default-items";
    public const string Dungeon = "dungeon";
    public const string EnchantLimit = "enchant-limit";
    public const string EnchantScroll = "enchant-scroll";
    public const string ExpTable = "exp-table";
    public const string FieldWar = "field-war";
    public const string FishingRod = "fishing-rod";
    public const string FishingSpot = "fishing-spot";
    public const string Fish = "fish";
    public const string FunctionCube = "function-cube";
    public const string FurnishingShop = "furnishing-shop";
    public const string Gacha = "gacha";
    public const string GuildBuff = "guild-buff";
    public const string GuildContribution = "guild-contribution";
    public const string GuildHouse = "guild-house";
    public const string GuildProperty = "guild-property";
    public const string GuildService = "guild-service";
    public const string HomeTemplate = "home-template";
    public const string Insignia = "insignia";
    public const string InstrumentCategoryInfo = "instrument-category-info";
    public const string InstrumentInfo = "instrument-info";
    public const string InteractObject = "interact-object";
    public const string ItemDrop = "item-drop";
    public const string ItemEnchantTransfer = "item-enchant-transfer";
    public const string ItemExchangeScroll = "item-exchange-scroll";
    public const string ItemExtraction = "item-extraction";
    public const string ItemGemstoneUpgrade = "item-gemstone-upgrade";
    public const string ItemOptionConstant = "item-option-constant";
    public const string ItemOptionPick = "item-option-pick";
    public const string ItemOptionRandom = "item-option-random";
    public const string ItemOptionRange = "item-option-range";
    public const string ItemOptionStatic = "item-option-static";
    public const string Item = "item";
    public const string ItemRepackage = "item-repackage";
    public const string ItemSocket = "item-socket";
    public const string ItemSocketScroll = "item-socket-scroll";
    public const string Job = "job";
    public const string MagicPath = "magic-path";
    public const string Map = "map";
    public const string MasteryFactor = "mastery-factor";
    public const string Mastery = "mastery";
    public const string MasteryUGCHousing = "mastery-ugc-housing";
    public const string MeretMarketCategory = "meret-market-category";
    public const string Mesh = "mesh";
    public const string Mount = "mount";
    public const string Npc = "npc";
    public const string PremiumClubDailyBenefit = "premium-club-daily-benefit";
    public const string PremiumClubEffect = "premium-club-effect";
    public const string PremiumClubPackage = "premium-club-package";
    public const string Prestige = "prestige";
    public const string PrestigeLevelMission = "prestige-level-mission";
    public const string Quest = "quest";
    public const string Recipe = "recipe";
    public const string RewardContent = "reward-content";
    public const string Script = "script";
    public const string Skill = "skill";
    public const string SurvivalGoldPassReward = "survival-gold-pass-reward";
    public const string SurvivalLevel = "survival-level";
    public const string SurvivalPeriod = "survival-period";
    public const string SurvivalSilverPassReward = "survival-silver-pass-reward";
    public const string Title = "title";
    public const string Trophy = "trophy";
    public const string UGCDesign = "ugc-design";
    public const string UGCMap = "ugc-map";

    public static string FullMetadataName(string name) => $"ms2-{name}-metadata";
}
