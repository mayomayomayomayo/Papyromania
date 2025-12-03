using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Cards
{
    public static Dictionary<string, CardDefinition> byName;
    public static Dictionary<int, CardDefinition> byID;

    public static void Load()
    {
        byName = new();
        byID = new();

        string json = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "cards.json"));

        CardData[] rawCards = JsonUtility.FromJson<CardDataArray>(json).cards;

        for (int i = 0; i < rawCards.Length; i++)
        {
            CardData raw = rawCards[i];
            CardDefinition card = (CardDefinition) Activator.CreateInstance(raw.type.ParseSubclass(), raw);
            
            byID[i] = card;
            byName[raw.name] = card;

            Debug.Log($"Successfully loaded {raw.name}");
        }
    }

    public static CardDefinition Get(string match) => byName.TryGetValue(match, out CardDefinition got) ? got : null;
    public static CardDefinition Get(int match) => byID.TryGetValue(match, out CardDefinition got) ? got : null;
}