using UnityEngine;

public static class AppStart
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadEverything()
    {
        ModLoader.LoadAllMods();
        Cards.Load();
        PathManager.LoadAllSegments();
    }
}