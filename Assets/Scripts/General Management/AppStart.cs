using UnityEngine;

public static class AppStart
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadEverything()
    {
        ModLoader.LoadAllMods();
        RuntimeCardLoader.LoadCardsFromJson();
        PathManager.LoadAllSegments();
    }
}