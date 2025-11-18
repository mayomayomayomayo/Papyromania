using System.Collections.Generic;
using System;
using UnityEngine;

public static class CardManager
{
    public static List<int> objectIDsInUse;

    public static void StartUp()
    {
        objectIDsInUse = new();
        RuntimeCardLoader.LoadCardsFromJson();
        LoadCards();
    }

    public static void LoadCards()
    {
        foreach (CardData cd in RuntimeCardLoader.rawCards)
        {
            CardObject card = CardMaker.NewCardCanvas().InitializeCard(cd);
            Cards.Add(card);
        }
    }
}

public static class CardMaker
{
    private static GameObject _cardCanvasPrefab;

    public static GameObject CardCanvasPrefab { get { return _cardCanvasPrefab ??= Resources.Load<GameObject>("Prefabs/CardCanvas"); } }

    public static Type ParseSubclass(this string typeStr) => Type.GetType($"{typeStr}CardObject") ?? throw new Exception($"Couldn't parse type {typeStr}");

    public static CardObject.CardType ParseCardType(this string typeStr) => Enum.TryParse(typeStr, true, out CardObject.CardType result) ? result : throw new Exception($"Couldn't parse CardType {typeStr}");

    public static GameObject NewCardCanvas() => UnityEngine.Object.Instantiate(CardCanvasPrefab); // GameObject.Instantiate looks better, but whatever God says i guess.

    public static CardObject InitializeCard(this GameObject canvas, CardData data)
    {
        Type subclass = data.type.ParseSubclass();
        CardObject cardObj = (CardObject)canvas.AddComponent(subclass);

        if (cardObj is ICardObject ico)
        {
            ico.InitializeValues(data);
            ico.UpdateObjectValues();
        }
        else throw new Exception("how");

        return cardObj;
    }
}

public static class Cards
{
    // A vestige of when i tried to also make it ID-based
    public static Dictionary<string, CardObject> byName = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void ClearCards() => byName = new();

    public static void Add(CardObject c) => byName[c.cardName] = c;    

    public static CardObject Get(string name) => byName.TryGetValue(name, out CardObject card) ? card : null;
}