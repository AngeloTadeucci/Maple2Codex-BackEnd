using GameParser.Tools;
using Maple2Storage.Enums;
using Maple2Storage.Types;

namespace GameParser.Parsers;

public static class StatsParser {
    public static void ParseStats(Item item, out List<(Stat stat, string description)> constantStats,
        out List<(StatRange stat, string description)> staticStats,
        out List<(StatRange stat, string description)> randomStats,
        out int randomStatCount
    ) {
        constantStats = [];
        staticStats = [];
        randomStats = [];
        randomStatCount = 0;
        if (item.Rarity is 0 or > 6) {
            return;
        }

        ConstantStats.GetStats(item, out Dictionary<StatAttribute, Stat> constantStats2);
        foreach ((StatAttribute _, Stat value) in constantStats2) {
            string stringKey = $"{StatToString(value.ItemAttribute)}_{(value.AttributeType is StatAttributeType.Flat ? "v" : "r")}";

            if (StringCommonParser.Get(stringKey, out string? description)) {
                constantStats.Add((value, description!));
            }
        }

        StaticStats.GetStats(item, out Dictionary<StatAttribute, StatRange> staticStats2);
        foreach ((StatAttribute _, StatRange value) in staticStats2) {
            string stringKey = $"{StatToString(value.ItemAttribute)}_{(value.AttributeType is StatAttributeType.Flat ? "v" : "r")}";

            if (StringCommonParser.Get(stringKey, out string? description)) {
                staticStats.Add((value, description!));
            }
        }

        RandomStats.GetStats(item, out Dictionary<StatAttribute, StatRange> randomStats2, out randomStatCount);
        foreach ((StatAttribute _, StatRange value) in randomStats2) {
            string stringKey = $"{StatToString(value.ItemAttribute)}_{(value.AttributeType is StatAttributeType.Flat ? "v" : "r")}";

            if (StringCommonParser.Get(stringKey, out string? description)) {
                randomStats.Add((value, description!));
            }
        }
    }

    private static string StatToString(StatAttribute stat) => stat switch {
        StatAttribute.Defense => "s_word_stat_ndd",
        StatAttribute.Str => "s_word_stat_str",
        StatAttribute.Dex => "s_word_stat_dex",
        StatAttribute.Int => "s_word_stat_int",
        StatAttribute.Luk => "s_word_stat_luk",
        StatAttribute.Hp => "s_word_stat_hp",
        StatAttribute.PhysicalAtk => "s_word_stat_pap",
        StatAttribute.MagicAtk => "s_word_stat_map",
        StatAttribute.PhysicalRes => "s_word_stat_par",
        StatAttribute.MagicRes => "s_word_stat_mar",
        StatAttribute.CritRate => "s_word_stat_cap",
        StatAttribute.PerfectGuard => "s_word_stat_abp",
        StatAttribute.MinWeaponAtk => "s_word_stat_wap_d",
        StatAttribute.MaxWeaponAtk => "s_word_stat_wap_u",
        StatAttribute.HpRegen => "s_word_stat_hp_rgp",
        StatAttribute.HpRegenInterval => "s_word_stat_hp_inv",
        StatAttribute.Spirit => "s_word_stat_sp",
        StatAttribute.SpRegen => "s_word_stat_hp_rgp",
        StatAttribute.SpRegenInterval => "s_word_stat_hp_inv",
        StatAttribute.Stamina => "s_word_stat_ep",
        StatAttribute.StaminaRegen => "s_word_stat_ep_rgp",
        StatAttribute.StaminaRegenInterval => "s_word_stat_ep_inv",
        StatAttribute.AttackSpeed => "s_word_stat_asp",
        StatAttribute.MovementSpeed => "s_word_stat_msp",
        StatAttribute.Accuracy => "s_word_stat_atp",
        StatAttribute.Evasion => "s_word_stat_evp",
        StatAttribute.CritDamage => "s_word_stat_cad",
        StatAttribute.CritEvasion => "s_word_stat_car",
        StatAttribute.JumpHeight => "s_word_stat_jmp",
        // StatAttribute.MinDamage => expr,
        // StatAttribute.MaxDamage => expr,
        StatAttribute.Pierce => "s_word_stat_pen",
        // StatAttribute.MountMovementSpeed => expr,
        StatAttribute.BonusAtk => "s_word_stat_bap",
        StatAttribute.PetBonusAtk => "s_word_stat_bap_pet",
        StatAttribute.ExpBonus => "s_item_opt_sa_improve_acquire_exp",
        StatAttribute.MesoBonus => "s_item_opt_sa_improve_acquire_meso",
        StatAttribute.SwimSpeed => "s_item_opt_sa_improve_speed_swim",
        StatAttribute.DashDistance => "s_item_opt_sa_improve_speed_dash",
        StatAttribute.TonicDropRate => "s_item_opt_sa_improve_acquire_potion",
        // StatAttribute.GearDropRate => "s_item_opt_sa_improve_acquire_equipment",
        StatAttribute.TotalDamage => "s_item_opt_sa_improve_damage_final",
        StatAttribute.CriticalDamage => "s_item_opt_sa_improve_damage_critical",
        StatAttribute.Damage => "s_item_opt_sa_improve_damage_normalNpc",
        StatAttribute.LeaderDamage => "s_item_opt_sa_improve_damage_leaderNpc",
        StatAttribute.EliteDamage => "s_item_opt_sa_improve_damage_namedNpc",
        StatAttribute.BossDamage => "s_item_opt_sa_improve_damage_bossNpc",
        StatAttribute.HpOnKill => "s_item_opt_sa_improve_recovery_hp_dokill",
        StatAttribute.SpiritOnKill => "s_item_opt_sa_improve_recovery_sp_dokill",
        StatAttribute.StaminaOnKill => "s_item_opt_sa_improve_recovery_ep_dokill",
        StatAttribute.Heal => "s_item_opt_sa_improve_recovery_regen_doheal",
        StatAttribute.AllyRecovery => "s_item_opt_sa_improve_recovery_regen_receiveheal",
        StatAttribute.IceDamage => "s_item_opt_sa_improve_elements_ice",
        StatAttribute.FireDamage => "s_item_opt_sa_improve_elements_fire",
        StatAttribute.DarkDamage => "s_item_opt_sa_improve_elements_dark",
        StatAttribute.HolyDamage => "s_item_opt_sa_improve_elements_light",
        StatAttribute.PoisonDamage => "s_item_opt_sa_improve_elements_poison",
        StatAttribute.ElectricDamage => "s_item_opt_sa_improve_elements_thunder",
        StatAttribute.MeleeDamage => "s_item_opt_sa_improve_damage_nearrange",
        StatAttribute.RangedDamage => "s_item_opt_sa_improve_damage_longrange",
        StatAttribute.PhysicalPiercing => "s_item_opt_sa_improve_piercing_par",
        StatAttribute.MagicPiercing => "s_item_opt_sa_improve_piercing_mar",
        StatAttribute.IceDamageReduce => "s_item_opt_sa_reduce_elements_ice",
        StatAttribute.FireDamageReduce => "s_item_opt_sa_reduce_elements_fire",
        StatAttribute.DarkDamageReduce => "s_item_opt_sa_reduce_elements_dark",
        StatAttribute.HolyDamageReduce => "s_item_opt_sa_reduce_elements_light",
        StatAttribute.PoisonDamageReduce => "s_item_opt_sa_reduce_elements_poison",
        StatAttribute.ElectricDamageReduce => "s_item_opt_sa_reduce_elements_thunder",
        StatAttribute.StunReduce => "s_item_opt_sa_reduce_time_stun",
        StatAttribute.CooldownReduce => "s_item_opt_sa_reduce_time_cooldown",
        StatAttribute.DebuffDurationReduce => "s_item_opt_sa_reduce_time_condition",
        StatAttribute.MeleeDamageReduce => "s_item_opt_sa_reduce_damage_nearrange",
        StatAttribute.RangedDamageReduce => "s_item_opt_sa_reduce_damage_longrange",
        StatAttribute.KnockbackReduce => "s_item_opt_sa_reduce_distance_knockBack",
        // StatAttribute.MeleeStun => expr,
        // StatAttribute.RangedStun => expr,
        // StatAttribute.MeeleeKnockback => expr,
        // StatAttribute.RangedKnockback => expr,
        // StatAttribute.MeleeImmob => expr,
        // StatAttribute.RangedImmob => expr,
        // StatAttribute.MeleeAoeDamage => expr,
        // StatAttribute.RangedAoeDamage => expr,
        StatAttribute.DropRate => "s_item_opt_sa_improve_npckill_dropitem_incrate",
        StatAttribute.QuestExp => "s_item_opt_sa_improve_acquire_questreward_exp",
        StatAttribute.QuestMeso => "s_item_opt_sa_improve_acquire_questreward_meso",
        // StatAttribute.InvokeEffect1 => expr,
        // StatAttribute.InvokeEffect2 => expr,
        // StatAttribute.InvokeEffect3 => expr,
        StatAttribute.PvPDamage => "s_item_opt_sa_improve_damage_pvp",
        StatAttribute.PvPDefense => "s_item_opt_sa_reduce_damage_pvp",
        StatAttribute.GuildExp => "s_item_opt_sa_improve_guild_exp",
        StatAttribute.GuildCoin => "s_item_opt_sa_improve_guild_coin",
        StatAttribute.McKayXpOrb => "s_item_opt_sa_improve_massive_event_expball",
        StatAttribute.FishingExp => "s_item_opt_sa_improve_acquire_fishing_exp",
        StatAttribute.ArcadeExp => "s_item_opt_sa_improve_acquire_arcade_exp",
        StatAttribute.PerformanceExp => "s_item_opt_sa_improve_acquire_playinstrument_exp",
        // StatAttribute.AssistantMood => expr,
        // StatAttribute.AssistantDiscount => expr,
        // StatAttribute.BlackMarketReduce => expr,
        // StatAttribute.EnchantCatalystDiscount => expr,
        // StatAttribute.MeretReviveFee => expr,
        // StatAttribute.MiningBonus => expr,
        // StatAttribute.RanchingBonus => expr,
        // StatAttribute.SmithingExp => expr,
        // StatAttribute.HandicraftMastery => expr,
        // StatAttribute.ForagingBonus => expr,
        // StatAttribute.FarmingBonus => expr,
        // StatAttribute.AlchemyMastery => expr,
        // StatAttribute.CookingMastery => expr,
        // StatAttribute.ForagingExp => expr,
        // StatAttribute.CraftingExp => expr,
        // StatAttribute.TECH => expr,
        // StatAttribute.TECH_2 => expr,
        // StatAttribute.TECH_10 => expr,
        // StatAttribute.TECH_13 => expr,
        // StatAttribute.TECH_16 => expr,
        // StatAttribute.TECH_19 => expr,
        // StatAttribute.TECH_22 => expr,
        // StatAttribute.TECH_25 => expr,
        // StatAttribute.TECH_28 => expr,
        // StatAttribute.TECH_31 => expr,
        // StatAttribute.TECH_34 => expr,
        // StatAttribute.TECH_37 => expr,
        // StatAttribute.TECH_40 => expr,
        // StatAttribute.TECH_43 => expr,
        StatAttribute.OXQuizExp => "s_item_opt_sa_improve_massive_ox_exp",
        StatAttribute.TrapMasterExp => "s_item_opt_sa_improve_massive_trapmaster_exp",
        StatAttribute.SoleSurvivorExp => "s_item_opt_sa_improve_massive_finalsurvival_exp",
        StatAttribute.CrazyRunnerExp => "s_item_opt_sa_improve_massive_crazyrunner_exp",
        StatAttribute.LudiEscapeExp => "s_item_opt_sa_improve_massive_sh_crazyrunner_exp",
        StatAttribute.SpringBeachExp => "s_item_opt_sa_improve_massive_escape_exp",
        StatAttribute.DanceDanceExp => "s_item_opt_sa_improve_massive_springbeach_exp",
        StatAttribute.OXMovementSpeed => "s_item_opt_sa_improve_massive_dancedance_exp",
        StatAttribute.TrapMasterMovementSpeed => "s_item_opt_sa_improve_massive_trapmaster_msp",
        StatAttribute.SoleSurvivorMovementSpeed => "s_item_opt_sa_improve_massive_finalsurvival_msp",
        StatAttribute.CrazyRunnerMovementSpeed => "s_item_opt_sa_improve_massive_crazyrunner_msp",
        StatAttribute.LudiEscapeMovementSpeed => "s_item_opt_sa_improve_massive_sh_crazyrunner_msp",
        StatAttribute.SpringBeachMovementSpeed => "s_item_opt_sa_improve_massive_escape_msp",
        StatAttribute.DanceDanceStopMovementSpeed => "s_item_opt_sa_improve_massive_springbeach_msp",
        StatAttribute.GenerateSpiritOrbs => "s_item_opt_sa_npc_hit_reward_sp_ball",
        StatAttribute.GenerateStaminaOrbs => "s_item_opt_sa_npc_hit_reward_ep_ball",
        StatAttribute.ValorTokens => "s_item_opt_sa_improve_honor_token",
        StatAttribute.PvPExp => "s_item_opt_sa_improve_pvp_exp",
        StatAttribute.DarkDescentDamageBonus => "s_item_opt_sa_improve_darkstream_damage",
        StatAttribute.DarkDescentDamageReduce => "s_item_opt_sa_reduce_darkstream_recive_damage",
        StatAttribute.DarkDescentEvasion => "s_item_opt_sa_improve_darkstream_evp",
        StatAttribute.DoubleFishingMastery => "s_item_opt_sa_fishing_double_mastery",
        StatAttribute.DoublePerformanceMastery => "s_item_opt_sa_playinstrument_double_mastery",
        StatAttribute.ExploredAreasMovementSpeed => "s_item_opt_sa_complete_fieldmission_msp",
        StatAttribute.AirMountAscentSpeed => "s_item_opt_sa_improve_glide_vertical_velocity",
        // StatAttribute.EnemyDefenseDecreaseOnHit => expr,
        // StatAttribute.EnemyAttackDecreaseOnHit => expr,
        // StatAttribute.IncreaseTotalDamageIf1NearbyEnemy => expr,
        // StatAttribute.IncreaseTotalDamageIf3NearbyEnemies => expr,
        // StatAttribute.IncreaseTotalDamageIf80Spirit => expr,
        // StatAttribute.IncreaseTotalDamageIfFullStamina => expr,
        // StatAttribute.IncreaseTotalDamageIfHerbEffectActive => expr,
        // StatAttribute.IncreaseTotalDamageToWorldBoss => expr,
        // StatAttribute.Effect95000026 => expr,
        // StatAttribute.Effect95000027 => expr,
        // StatAttribute.Effect95000028 => expr,
        // StatAttribute.Effect95000029 => expr,
        StatAttribute.StaminaRecoverySpeed => "s_item_opt_sa_reduce_recovery_ep_inv",
        StatAttribute.MaxWeaponAttack => "s_item_opt_sa_improve_stat_wap_u",
        StatAttribute.DoubleMiningProduction => "s_item_opt_sa_mining_double_reward",
        StatAttribute.DoubleRanchingProduction => "s_item_opt_sa_breeding_double_reward",
        StatAttribute.DoubleForagingProduction => "s_item_opt_sa_gathering_double_reward",
        StatAttribute.DoubleFarmingProduction => "s_item_opt_sa_farming_double_reward",
        StatAttribute.DoubleSmithingProduction => "s_item_opt_sa_blacksmithing_double_reward",
        StatAttribute.DoubleHandicraftProduction => "s_item_opt_sa_engraving_double_reward",
        StatAttribute.DoubleAlchemyProduction => "s_item_opt_sa_alchemist_double_reward",
        StatAttribute.DoubleCookingProduction => "s_item_opt_sa_cooking_double_reward",
        StatAttribute.DoubleMiningMastery => "s_item_opt_sa_mining_double_mastery",
        StatAttribute.DoubleRanchingMastery => "s_item_opt_sa_breeding_double_mastery",
        StatAttribute.DoubleForagingMastery => "s_item_opt_sa_gathering_double_mastery",
        StatAttribute.DoubleFarmingMastery => "s_item_opt_sa_farming_double_mastery",
        StatAttribute.DoubleSmithingMastery => "s_item_opt_sa_blacksmithing_double_mastery",
        StatAttribute.DoubleHandicraftMastery => "s_item_opt_sa_engraving_double_mastery",
        StatAttribute.DoubleAlchemyMastery => "s_item_opt_sa_alchemist_double_mastery",
        StatAttribute.DoubleCookingMastery => "s_item_opt_sa_cooking_double_mastery",
        StatAttribute.ChaosRaidWeaponAttack => "s_item_opt_sa_improve_chaosraid_wap",
        StatAttribute.ChaosRaidAttackSpeed => "s_item_opt_sa_improve_chaosraid_asp",
        StatAttribute.ChaosRaidAccuracy => "s_item_opt_sa_improve_chaosraid_atp",
        StatAttribute.ChaosRaidHealth => "s_item_opt_sa_improve_chaosraid_hp",
        StatAttribute.StaminaAndSpiritFromOrbs => "s_item_opt_sa_improve_recovery_ball",
        StatAttribute.WorldBossExp => "s_item_opt_sa_improve_fieldboss_kill_exp",
        StatAttribute.WorldBossDropRate => "s_item_opt_sa_improve_fieldboss_kill_drop",
        StatAttribute.WorldBossDamageReduce => "s_item_opt_sa_reduce_fieldboss_recive_damage",
        StatAttribute.Effect9500016 => "s_item_opt_sa_additionaleffect_95000016",
        StatAttribute.PetCaptureRewards => "s_item_opt_sa_improve_pettrap_reward",
        StatAttribute.MiningEfficency => "s_item_opt_sa_ming_multiaction",
        StatAttribute.RanchingEfficiency => "s_item_opt_sa_breeding_multiaction",
        StatAttribute.ForagingEfficiency => "s_item_opt_sa_gathering_multiaction",
        StatAttribute.FarmingEfficiency => "s_item_opt_sa_farming_multiaction",
        // StatAttribute.ShanghaiCrazyRunnersExp => expr,
        // StatAttribute.ShanghaiCrazyRunnersMovementSpeed => expr,
        // StatAttribute.HealthBasedDamageReduce => expr,
        _ => throw new ArgumentOutOfRangeException(nameof(stat), stat, $"Unhandled stat: {stat}")
    };
}
