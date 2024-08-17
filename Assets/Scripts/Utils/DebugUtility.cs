using UnityEngine.Assertions;

// Helper static class with a bunch of convenience
// functions for testing and debugging
public static class DebugUtility
{
    public static void Assert(bool condition, string message)
    {
        UnityEngine.Assertions.Assert.IsTrue(condition, message);
    }

    public static void AssertNotNull<T>(T obj, string message)
        where T : class
    {
        UnityEngine.Assertions.Assert.IsNotNull(obj, message);
    }

    public static void AssertNull<T>(T obj, string message)
        where T : class
    {
        UnityEngine.Assertions.Assert.IsNull(obj, message);
    }

    // A fatal path of code execution was taken
    public static void Fatal(string message)
    {
        throw new AssertionException(message, message);
    }

    public static string FatalExpr(string message)
    {
        Fatal(message);

        return string.Empty;
    }
}