using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Cards
{
    private static Dictionary<string, CardDefinition> byName;
    public static List<CardDefinition> cards;

    public static void Load()
    {
        byName = new();
        cards = new();

        string json = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "cards.json"));

        CardData[] rawCards = JsonUtility.FromJson<CardDataArray>(json).cards;

        for (int i = 0; i < rawCards.Length; i++)
        {
            CardData raw = rawCards[i];
            CardDefinition cd = (CardDefinition) Activator.CreateInstance(raw.type.ParseSubclass(), raw);
            
            cards.Add(cd);
            byName[raw.name] = cd;

            Debug.Log($"Successfully loaded {raw.name}");
        }
    }

    public static CardDefinition Get(string match) => byName.TryGetValue(match, out CardDefinition got) ? got : null;
    public static CardDefinition Get(int match) => cards[match];

    public static Card CreateCard(CardDefinition def, Vector3? pos = null)
    {
        Vector3 position = pos ?? Vector3.zero;

        GameObject cardObject = CardUtils.NewCard(position);

        Card card = cardObject.AddComponent<Card>();

        card.definition = def;
        card.behaviour = (CardBehaviour)cardObject.AddComponent(def.BehaviourType);
        card.visuals = cardObject.AddComponent<CardVisuals>();

        card.behaviour.owner = card;
        card.visuals.owner = card;

        return card;
    }
}

public static class CardUtils
{
    private static GameObject _cardCanvasPrefab; 
    public static GameObject CardCanvasPrefab { get { return _cardCanvasPrefab = _cardCanvasPrefab != null ? _cardCanvasPrefab : Resources.Load<GameObject>("Prefabs/CardCanvas"); } }

    public static GameObject NewCard(Vector3 pos) => UnityEngine.Object.Instantiate(CardCanvasPrefab, pos, Quaternion.identity);

    public static CardDefinition.CardType ParseCardType(this string typeStr) => Enum.TryParse(typeStr, true, out CardDefinition.CardType result) ? result : throw new Exception($"Couldn't parse CardType {typeStr}");
    
    public static Type ParseSubclass(this string typeStr) => Type.GetType($"{typeStr}CardDefinition") ?? throw new Exception($"Couldn't parse type {typeStr}");
}