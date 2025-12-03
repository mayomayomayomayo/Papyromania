using System;
using UnityEngine;

[Serializable]
public abstract class CardDefinition
{
    [Header("Identity")]
    public int ID;

    [Header("Values")]
    public CardType type;
    public CardModifiers modifiers;

    [Header("Behaviour")]
    public Type BehaviourType { get; }

    [Header("Visual data")]
    public string name;
    public string description;
    public string leftFieldText;
    public string rightFieldText;
    public Sprite cardBase;
    public Sprite cardArt;
    
    public CardDefinition(CardData raw)
    {
        type = raw.type.ParseCardType();
        name = raw.name;
        leftFieldText = raw.leftFieldText;
        rightFieldText = raw.rightFieldText;
        description = raw.description;
        cardBase = Resources.Load<Sprite>($"Sprites/{raw.type}_Card_Template");
        cardArt = Resources.Load<Sprite>(raw.pathToSprite);
        
        modifiers = CardModifiers.None;
        foreach (string modString in raw.mods)
        {
            if (Enum.TryParse(modString, true, out CardModifiers mod)) modifiers |= mod;
            else throw new Exception($"{name} - Couldn't parse invalid modifier: {modString}");
        }

        BehaviourType = ModLoader.knownCustomCardBehaviours.TryGetValue(raw.customBehaviourType ?? string.Empty, out Type customBehaviourType) ? 
            customBehaviourType :
            Type.GetType($"{type}CardBehaviour");
    }

    public enum CardType
    {
        Gun,
        Consumable
    }

    [Flags]
    public enum CardModifiers
    {
        None = 0,
        NonReloadable = 1 << 0,
        NonUsable = 1 << 1,
        NonNaturallyDroppable = 1 << 2,
        AutoDiscardOnEmpty = 1 << 3,
        NonDiscardable = 1 << 4,
        NonUsablePrimary = 1 << 5,
        NonUsableSecondary = 1 << 6,
    }
}