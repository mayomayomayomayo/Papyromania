using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

public static class Cards
{
    private static readonly Dictionary<CardID, Card> _cards = new();

    public static Card Get(CardID cid)
    {
        if (_cards.TryGetValue(cid, out Card c))
        {
            return c;
        }
        return null;
    }
    
    public static Card Get(string idStr) => Get((CardID)idStr);

    public static IEnumerable<Card> AllCards => _cards.Values;

    public static void LoadCards()
    {
        // TODO: Add mod support!!! Next update!!! I promise!!!!!
        string json = File.ReadAllText(
            Path.Combine(
                Application.streamingAssetsPath, 
                "Cards", 
                "cards.json"
            )
        );

        JsonSerializerSettings settings = new()
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        Card[] cards = JsonConvert.DeserializeObject<Card[]>(json, settings);

        foreach (Card card in cards)
        {
            _cards[card.ID] = card;

            // DEBUG!!!!
            CardSpawner.SpawnCard(card, Vector3.zero, Quaternion.identity);
        }
    }
}

public static class CardSpawner
{
    class CardVessel : MonoVessel<Card> {}

    private static readonly GameObject _canvasPrefab = Resources.Load<GameObject>("Prefabs/CardCanvas");

    public static GameObject SpawnCard(Card card, Vector3 pos, Quaternion rot)
    {
        GameObject go = Object.Instantiate(_canvasPrefab, pos, rot);

        go.name = card.Name;

        go.AddComponent<CardVessel>().Init(card);

        go.GetComponent<CardFields>().UpdateFields(card);

        return go;
    }
}