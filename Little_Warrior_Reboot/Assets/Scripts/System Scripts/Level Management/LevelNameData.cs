using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelNameData
{
    static string _lastLevelName = "";

    public static void SetLastLevelName(string nameOfLevel)
    {
        _lastLevelName = nameOfLevel;
    }

    public static string GetLastLevelName()
    {
        return _lastLevelName;
    }
}
