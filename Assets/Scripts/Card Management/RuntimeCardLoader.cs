using UnityEngine;
using /*Old McDonald had a*/ System. /*I-A-I-A*/ IO;
using System;

public static class RuntimeCardLoader
{
    public static CardData[] rawCards;

    public static void LoadCardsFromJson() // Call this from GameManager -> OnAwake
    {
        rawCards = Array.Empty<CardData>(); // Static data structures only clear on domain reload, wtf Unity?

        string json = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "cards.json"));

        rawCards = JsonUtility.FromJson<CardDataArray>(json).cards;
    }
}

[Serializable]
public class CardData // Wrapper class for the card data cause JsonUtility is kinda stupid like that
{
    [Header("General")]
    public string type;
    public string name;
    public string description;
    public string pathToSprite;
    public string[] mods;
    public int id;

    [Header("Format")]
    public string leftFieldText;
    public string rightFieldText;

    [Header("GunCardObject")]
    public float damage;
    public int ammo;
}

[Serializable]
public class CardDataArray // Wrapper class for an array of the already-wrapper class, are we deadass JsonUtil
{
    public CardData[] cards;
}