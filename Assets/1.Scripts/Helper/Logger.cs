using System.Diagnostics;

public static class Logger
{
    [Conditional("ENABLE_LOGS")]
    public static void Log(string message)
    {
        UnityEngine.Debug.Log(message);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogError(string message)
    {
        UnityEngine.Debug.LogError(message);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogWarning(string message)
    {
        UnityEngine.Debug.LogWarning(message);
    }
}
