using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace GameParser;

public static class QueryManager
{
    public static QueryFactory QueryFactory
    {
        get
        {
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

    private static MySqlConnection Connection()
    {
        return new("SERVER=localhost;PORT=3306;USER=root;PASSWORD=password;DATABASE=maple2_codex;");
    }

    public static MySqlConnection ConnectionNoDb()
    {
        return new("SERVER=localhost;PORT=3306;USER=root;PASSWORD=password;");
    }
}