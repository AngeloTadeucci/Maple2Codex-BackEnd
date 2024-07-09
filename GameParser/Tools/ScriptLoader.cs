using System.Collections.Concurrent;
using Maple2Storage.Types;
using MoonSharp.Interpreter;

namespace GameParser.Tools;

public static class ScriptLoader {
    private static readonly ConcurrentDictionary<string, Script> Scripts = new();

    /// <summary>
    /// Get the script from the cache or load it if it's not in the cache.
    /// If a session is provided, it'll create a new script every time.
    /// </summary>
    /// <returns><see cref="Script"/></returns>
    public static Script? GetScript(string scriptName) {
        // If session is null, use the cached script.
        if (Scripts.TryGetValue(scriptName, out Script? script)) {
            return script;
        }

        // If the script is not in the cache, create a new script.
        if (!NewScript(scriptName, out script)) {
            return null;
        }

        if (script == null) {
            return null;
        }

        Scripts.TryAdd(scriptName, script);
        return script;
    }

    /// <summary>
    /// Calls the specified function.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public static DynValue? RunFunction(this Script script, string functionName, params object[] args) {
        if (script.Globals[functionName] == null) {
            return null;
        }

        try {
            return script.Call(script.Globals[functionName], args);
        } catch (Exception) {
            return null;
        }
    }

    private static bool NewScript(string scriptName, out Script? script) {
        script = null;
        string scriptPath = $"{Paths.ScriptsDir}/{scriptName}.lua";
        if (!File.Exists(scriptPath)) {
            return false;
        }

        script = new();

        try {
            script.DoFile(scriptPath);
        } catch (Exception) {
            return false;
        }

        return true;
    }
}
