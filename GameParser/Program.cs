using GameParser;
using GameParser.Parsers;
using Maple2Storage.Tools;
using Maple2Storage.Types;
using MySql.Data.MySqlClient;

DotEnv.Load();

CreateDatabase();

Console.WriteLine("Parsing items...");
ItemParser.Parse();

Console.WriteLine("Parsing boxes...");
ItemDropParser.Parse();

Console.WriteLine("Parsing npcs...");
NpcParser.Parse();

Console.WriteLine("Finished!");

static void CreateDatabase() {
    string databaseSql = File.ReadAllText(Path.Combine(Paths.SolutionDir, "GameParser", "Database.sql"));
    MySqlScript script = new(QueryManager.ConnectionNoDb(), databaseSql);
    script.Execute();
}
