using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace GameParser;

public static class QueryManager {
    private static string ConnectionString { get; set; }
    private static string ConnectionStringNoDb { get; set; }

    static QueryManager() {
        string? server = Environment.GetEnvironmentVariable("DB_IP");
        string? port = Environment.GetEnvironmentVariable("DB_PORT");
        string? user = Environment.GetEnvironmentVariable("DB_USER");
        string? password = Environment.GetEnvironmentVariable("DB_PASSWORD");
        string? database = Environment.GetEnvironmentVariable("DB_NAME");

        if (server == null || port == null || database == null || user == null || password == null) {
            throw new ArgumentException("Database connection information was not set");
        }

        ConnectionStringNoDb = $"Server={server};Port={port};User={user};Password={password};";
        ConnectionString = $"{ConnectionStringNoDb}Database={database};";
    }

    public static QueryFactory QueryFactory {
        get {
            using MySqlConnection connection = Connection();
            QueryFactory queryFactory = new(connection, new MySqlCompiler());
            // Log the compiled query to the console
            // queryFactory.Logger = compiled =>
            // {
            //     Logger.Debug(compiled.ToString());
            // };
            return queryFactory;
        }
    }

    public static MySqlConnection Connection() {
        return new(ConnectionString);
    }

    public static MySqlConnection ConnectionNoDb() {
        return new(ConnectionStringNoDb);
    }
}
