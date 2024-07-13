using System.Text.RegularExpressions;
using GameParser.Parsers;

namespace GameParser.DescriptionHelper;

public static class Helper
{
    public static string FixDescription(string description)
    {
        MatchCollection matches = Regex.Matches(description, @"\$[a-zA-Z]+:[0-9]+\$");

        if (matches.Count == 0)
        {
            return description;
        }

        foreach (Match? match in matches)
        {
            if (string.IsNullOrEmpty(match?.ToString()))
            {
                continue;
            }

            string matchString = match.ToString()!;
            string[] split = matchString.Replace("$", string.Empty).Split(':');
            string key = split[0];
            if (!int.TryParse(split[1], out int value))
            {
                continue;
            }

            switch (key)
            {
                case "map":
                    string mapName = MapNameParser.MapNames.GetValueOrDefault(value) ?? string.Empty;
                    description = description.Replace(matchString, mapName);
                    break;

                case "item":
                    string itemName = ItemNameParser.ItemNames.GetValueOrDefault(value) ?? string.Empty;
                    description = description.Replace(matchString, itemName);
                    break;
                case "itemPlural":
                    string itemNamePlural = ItemNameParser.ItemNamesPlural.GetValueOrDefault(value) ?? string.Empty;
                    description = description.Replace(matchString, itemNamePlural);
                    break;
                case "npc":
                case "npcName":
                    string npcName = NpcNameParser.NpcNames.GetValueOrDefault(value) ?? string.Empty;
                    description = description.Replace(matchString, npcName);
                    break;

                case "npcPlural":
                case "npcNamePlural":
                    string npcNamePlural = NpcNameParser.NpcNamesPlural.GetValueOrDefault(value) ?? string.Empty;
                    description = description.Replace(matchString, npcNamePlural);
                    break;

                case "npcTitle":
                    string npcTitle = NpcNameParser.NpcTitles.GetValueOrDefault(value) ?? string.Empty;
                    description = description.Replace(matchString, npcTitle);
                    break;

                case "quest":
                    string questName = QuestNameParser.QuestNames.GetValueOrDefault(value)?.Name ?? string.Empty;
                    description = description.Replace(matchString, questName);
                    break;

                case "skill":
                    string skillName = SkillNameParser.SkillNames.GetValueOrDefault(value) ?? string.Empty;
                    description = description.Replace(matchString, skillName);
                    break;

                case "dungeonTitle":
                    string dungeonTitle = DungeonTitleParser.DungeonTitleNames.GetValueOrDefault(value).name;
                    description = description.Replace(matchString, dungeonTitle);
                    break;
            }
        }

        return description;
    }
}
