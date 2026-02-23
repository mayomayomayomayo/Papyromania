using UnityEngine;

public static class AppStart
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        ModLoader.LoadMods().GetAwaiter().GetResult();
        Cards.Load();
        PathManager.LoadAllSegments();

        foreach (DepCardDefinition cd in Cards.cards)
        {
            Cards.CreateCard(cd);
            Cards.CreateCard(cd);
        }
    }
}