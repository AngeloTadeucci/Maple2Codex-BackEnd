using System.Data;
using GameParser;
using GameParser.Parsers;
using Maple2Storage.Tools;
using Maple2Storage.Types;
using MySql.Data.MySqlClient;

DotEnv.Load();

// check if database exists
if (!DatabaseExists()) {
    Console.WriteLine("Database does not exist. Creating...");
    CreateDatabase();
} else {
    Console.WriteLine("Do you want to drop and create the whole database? (y/n)");
    if (Console.ReadLine()?.ToLower() == "y") {
        Console.WriteLine("Clearing database...");
        CreateDatabase();
    }
}

(string, Action)[] tables = [
    ("items", ItemParser.Parse),
    ("item_boxes", ItemDropParser.Parse),
    ("npcs", NpcParser.Parse),
    ("maps", MapNameParser.Parse),
    ("achieves", AchieveParser.Parse),
    ("additional_effects", AdditionalEffectParser.Parse)
];

foreach ((string, Action) table in tables) {
    if (!TableExists(table.Item1)) {
        Console.WriteLine($"{table.Item1} table does not exist. Creating...");
        DropAndCreateTable(table.Item1);
        table.Item2();
        continue;
    }

    Console.WriteLine($"Drop and create {table.Item1}? (y/n)");
    if (Console.ReadLine()?.ToLower() == "n") {
        continue;
    }
    DropAndCreateTable(table.Item1);
    table.Item2();
}

Console.WriteLine("Finished!");

static void CreateDatabase() {
    string databaseSql = File.ReadAllText(Path.Combine(Paths.SolutionDir, "GameParser", "SQL", "database.sql"));
    string databaseName = Environment.GetEnvironmentVariable("DB_NAME")!;
    MySqlScript script = new(QueryManager.ConnectionNoDb(), databaseSql.Replace("{databaseName}", databaseName));
    script.Execute();
}

static bool DatabaseExists() {
    // Obtain the connection object
    var connection = QueryManager.ConnectionNoDb();

    string? databaseName = Environment.GetEnvironmentVariable("DB_NAME");

    // Check if the database name is not null or empty
    if (string.IsNullOrEmpty(databaseName)) {
        throw new NullReferenceException("Database name is null or empty");
    }

    // Define the query to check if the database exists
    string query = $"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{databaseName}'";

    // Create the MySqlCommand object
    using (MySqlCommand command = new(query, connection)) {
        // Ensure the connection is open
        if (connection.State == ConnectionState.Closed) {
            connection.Open();
        }

        // Execute the query and check if the result is not null
        return command.ExecuteScalar() != null;
    }
}

static void DropAndCreateTable(string tableName) {
    string databaseSql = File.ReadAllText(Path.Combine(Paths.SolutionDir, "GameParser", "SQL", $"{tableName}.sql"));
    string databaseName = Environment.GetEnvironmentVariable("DB_NAME")!;

    MySqlScript script = new(QueryManager.Connection(), databaseSql.Replace("{databaseName}", databaseName));
    script.Execute();
}

static bool TableExists(string tableName) {
    var connection = QueryManager.ConnectionNoDb();

    string? databaseName = Environment.GetEnvironmentVariable("DB_NAME");

    string query = $"SELECT table_name FROM information_schema.tables WHERE table_schema = '{databaseName}' AND table_name = '{tableName}'";
    // Create the MySqlCommand object
    using (MySqlCommand command = new(query, connection)) {
        // Ensure the connection is open
        if (connection.State == ConnectionState.Closed) {
            connection.Open();
        }

        // Execute the query and check if the result is not null
        return command.ExecuteScalar() != null;
    }
}
