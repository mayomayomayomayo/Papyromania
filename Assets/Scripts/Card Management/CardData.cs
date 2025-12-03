using System;
using UnityEngine;

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
    public string customBehaviourType;

    [Header("Gun")]
    public float damage;
    public int maxAmmo;
    public bool isFullAuto;
    public float shotDelay;
    public float shotRange;
}

[Serializable] // This... really doesn't warrant its own class but okay.
public class CardDataArray
{
    public CardData[] cards;
}