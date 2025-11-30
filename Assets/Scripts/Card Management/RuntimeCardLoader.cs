using UnityEngine;
using /*Old McDonald had a*/ System. /*I-A-I-A*/ IO;
using System;

public static class RuntimeCardLoader
{
    public static CardData[] rawCards;

    public static void LoadCardsFromJson()
    {
        rawCards = Array.Empty<CardData>();

        string json = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "cards.json"));

        rawCards = JsonUtility.FromJson<CardDataArray>(json).cards;

        Cards.Load(rawCards);
    }
}

[Serializable]
public class CardData 
{
    [Header("General")]
    public string type;
    public string name;
    public string description;
    public string pathToSprite;
    public string[] mods;

    [Header("Format")]
    public string leftFieldText;
    public string rightFieldText;

    [Header("Behaviour")]
    public string customBehaviour;

    [Header("GunCardObject")]
    public float damage;
    public int ammo;
    public bool isFullAuto = false;
    public float shotDelay = 0.5f;
}

[Serializable] // This... really doesn't warrant its own class but okay.
public class CardDataArray
{
    public CardData[] cards;
}