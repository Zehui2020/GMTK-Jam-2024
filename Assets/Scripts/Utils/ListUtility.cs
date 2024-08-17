using System.Collections.Generic;

public static class ListUtility
{
    public static bool IsEmpty<T>(this List<T> list)
    {
        return list == null || list.Count == 0;
    }
}