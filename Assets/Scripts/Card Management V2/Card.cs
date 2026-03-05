using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public sealed class Card
{
    [JsonIgnore]
    public CardID ID { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public string LeftField { get; private set; }

    public string RightField { get; private set; }
    
    [JsonIgnore]
    public Sprite BaseSprite { get; private set; } 

    [JsonIgnore]
    public Sprite ArtSprite { get; private set; }

    [JsonIgnore]
    public Sprite LeftFieldIcon { get; private set; }

    [JsonIgnore]
    public Sprite RightFieldIcon { get; private set; }

    public CardEffect[] Effects { get; private set; }

    public void Use()
    {
        CardUseContext ctx = new(Effects);

        foreach (CardEffect effect in Effects)
            effect.Execute(ctx);
    }

    [JsonConstructor]
    public Card(
        string id,
        string name, 
        string description,
        string leftField,
        string rightField,
        string leftFieldIcon,
        string rightFieldIcon,
        string baseSprite,
        string artSprite,
        CardEffect[] effects
    )
    {
        // Add support for modded IDs!!!!
        ID = (CardID) $"zb.{id}";
        Name = name;
        Description = description;
        LeftField = leftField;
        RightField = rightField;
        Effects = effects;
        LeftFieldIcon = Resources.Load<Sprite>(leftFieldIcon);
        RightFieldIcon = Resources.Load<Sprite>(rightFieldIcon);
        BaseSprite = Resources.Load<Sprite>(baseSprite);
        ArtSprite = Resources.Load<Sprite>(artSprite);
    }
}

public readonly struct CardID : IEquatable<CardID>
{
    public readonly string Origin;

    public readonly string Name;

    public string ID => $"{Origin}.{Name}";

    private static readonly Regex _match = new (@"^[a-z0-9_]+\.[a-z0-9_]+$", RegexOptions.Compiled);

    public CardID(string origin, string name)
    {
        Origin = origin;
        Name = name;

        if (!_match.IsMatch(ID))
            throw new Exception($"Invalid CardID: {ID}");
    }

    public bool Equals(CardID other) =>
        Origin == other.Origin && Name == other.Name;

    public override bool Equals(object obj) =>
        obj is CardID other && Equals(other);

    public override int GetHashCode() =>
        HashCode.Combine(Origin, Name);

    public static implicit operator string(CardID cid) => cid.ID;
    public static explicit operator CardID(string str) =>
        TryParse(str, out var id) ? id : throw new Exception($"Invalid card ID: {str}");

    public static bool TryParse(string str, out CardID id)
    {
        id = default;

        if (string.IsNullOrWhiteSpace(str))
            return false;

        string[] split = str.Split('.', 2);

        if (split.Length != 2)
            return false;
        
        id = new(split[0], split[1]);
        return true;
    }

    public override string ToString() => ID;
}