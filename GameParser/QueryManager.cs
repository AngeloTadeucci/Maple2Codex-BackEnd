using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace GameParser;

public static class QueryManager {
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

        return new("SERVER=localhost;PORT=3310;USER=root;PASSWORD=password;DATABASE=maple2_codex;");
    public static MySqlConnection Connection() {
    }

    public static MySqlConnection ConnectionNoDb() {
        return new("SERVER=localhost;PORT=3310;USER=root;PASSWORD=password;");
    }
}
