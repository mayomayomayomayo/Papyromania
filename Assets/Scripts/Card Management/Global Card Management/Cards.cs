using System.Collections.Generic;

public static class Cards
{
    public static List<int> objectIDsInUse;
    
    public static Dictionary<string, CardObject> cards = new();

    public static void Load(CardData[] raw)
    {
        cards = new();
        objectIDsInUse = new();
        
        foreach (CardData cd in raw) Add(CardMaker.NewCardCanvas().InitializeCard(cd));
    }

    public static void Add(CardObject c) => cards[c.cardName] = c;    

    public static CardObject Get(string name) => cards.TryGetValue(name, out CardObject card) ? card : null;
}