using UnityEngine;

public static class AppStart
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        Cards.LoadCards();

        
        // Cards.Load();
        // PathManager.LoadAllSegments();

        // foreach (DepCardDefinition cd in Cards.byID)
        // {
        //     Cards.CreateCard(cd);
        //     Cards.CreateCard(cd);
        // }
    }
}