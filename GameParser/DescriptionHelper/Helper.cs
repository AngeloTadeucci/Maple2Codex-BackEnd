using System.Text.RegularExpressions;
using GameParser.Parsers;

namespace GameParser.DescriptionHelper;

public static class Helper {
    public static string FixDescription(string description) {
        if (string.IsNullOrEmpty(description)) {
            return description;
        }

        MatchCollection matches = Regex.Matches(description, @"\$[a-zA-Z]+:[0-9]+\$");

        if (matches.Count == 0) {
            return description;
        }

        foreach (Match? match in matches) {
            if (string.IsNullOrEmpty(match?.ToString())) {
                continue;
            }

            string matchString = match.ToString()!;
            string[] split = matchString.Replace("$", string.Empty).Split(':');
            string key = split[0];
            if (!int.TryParse(split[1], out int value)) {
                continue;
            }

            // TODO: Add some way to reuse the key after transforming it

            switch (key) {
                case "map":
                    if (MapNameParser.MapNames.TryGetValue(value, out string? mapName)) {
                        description = description.Replace(matchString, mapName);
                    }
                    break;

                case "item":
                    if (ItemNameParser.ItemNames.TryGetValue(value, out string? itemName)) {
                        description = description.Replace(matchString, itemName);
                    }
                    break;

                case "itemPlural":
                    if (ItemNameParser.ItemNamesPlural.TryGetValue(value, out string? itemNamePlural)) {
                        description = description.Replace(matchString, itemNamePlural);
                    }
                    break;

                case "npc":
                case "npcName":
                    if (NpcNameParser.NpcNames.TryGetValue(value, out string? npcName)) {
                        description = description.Replace(matchString, npcName);
                    }
                    break;

                case "npcPlural":
                case "npcNamePlural":
                    if (NpcNameParser.NpcNamesPlural.TryGetValue(value, out string? npcNamePlural)) {
                        description = description.Replace(matchString, npcNamePlural);
                    }
                    break;

                case "npcTitle":
                    if (NpcNameParser.NpcTitles.TryGetValue(value, out string? npcTitle)) {
                        description = description.Replace(matchString, npcTitle);
                    }
                    break;

                case "quest":
                    if (QuestNameParser.QuestNames.TryGetValue(value, out QuestNameParser.QuestDescription? questName)) {
                        description = description.Replace(matchString, questName?.Name);
                    }
                    break;

                case "skill":
                    if (SkillNameParser.SkillNames.TryGetValue(value, out string? skillName)) {
                        description = description.Replace(matchString, skillName);
                    }
                    break;

                case "dungeonTitle":
                    if (DungeonTitleParser.DungeonTitleNames.TryGetValue(value, out (string name, string uiDescription) dungeonTitle)) {
                        description = description.Replace(matchString, dungeonTitle.name);
                    }
                    break;
            }
        }

        return description;
    }
}
