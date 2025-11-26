using System;
using UnityEngine;

public static class CardMaker
{
    private static GameObject _cardCanvasPrefab;

    public static GameObject CardCanvasPrefab { get { return _cardCanvasPrefab ??= Resources.Load<GameObject>("Prefabs/CardCanvas"); } }

    public static Type ParseSubclass(this string typeStr) => Type.GetType($"{typeStr}CardObject") ?? throw new Exception($"Couldn't parse type {typeStr}");

    public static CardObject.CardType ParseCardType(this string typeStr) => Enum.TryParse(typeStr, true, out CardObject.CardType result) ? result : throw new Exception($"Couldn't parse CardType {typeStr}");

    public static GameObject NewCardCanvas() => UnityEngine.Object.Instantiate(CardCanvasPrefab); 

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

