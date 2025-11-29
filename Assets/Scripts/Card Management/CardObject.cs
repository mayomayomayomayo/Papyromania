using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardObject : MonoBehaviour, ICardObject
{
    public enum CardType
    {
        Gun,
        Consumable,
    }

    public enum CardState
    {
        InHand,
        Dropped,
        DroppedNonInteractable
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
    }

    [Header("Definition")]
    public CardType cardType;
    public string cardName;
    public string cardDescription;
    public Sprite cardBase;
    public Sprite cardArt;

    [Header("Fields")]
    public string cardRightFieldText;
    public string cardLeftFieldText;

    [Header("Meta")]
    public CardModifiers cardModifiers = CardModifiers.None;
    public CardState cardState = CardState.Dropped;
    public int ObjectID { get; private set; }

    [Header("References")]
    public TMP_Text nameField;
    public TMP_Text descriptionField;
    public TMP_Text leftField;
    public TMP_Text rightField;
    public Image baseImage;
    public Image artImage;
    public BoxCollider cardCollider;
    public CardPositionManager cardPositionManager;

    private List<CardObject> playerHand;

    public virtual void OnPlay() => Debug.Log($"{cardName} was played."); 

    public virtual void OnDiscard() => Debug.Log($"{cardName} was discarded."); 

    public virtual void OnTriggerEnter(Collider other)
    {
        if (cardState == CardState.Dropped && other.CompareTag("Player")) Pickup(other.gameObject);
    }

    private void Pickup(GameObject other)
    {
        playerHand = other.GetComponent<Player>().hand.handList;
        cardState = CardState.InHand;
        cardPositionManager.OnPickup(other);
        playerHand.Add(this);
        cardCollider.enabled = false;
    }

    public virtual void InitializeValues(CardData data) 
    {
        int oID;
        do oID = UnityEngine.Random.Range(0, int.MaxValue);
        while (Cards.objectIDsInUse.Contains(oID));
        ObjectID = oID;
        Cards.objectIDsInUse.Add(ObjectID);

        cardType = data.type.ParseCardType();
        cardName = data.name;
        cardDescription = data.description;
        cardRightFieldText = data.rightFieldText;
        cardLeftFieldText = data.leftFieldText;
        cardBase = Resources.Load<Sprite>($"Sprites/{data.type}_Card_Template");
        cardArt = Resources.Load<Sprite>(data.pathToSprite);

        foreach (string modStr in data.mods)
        {
            if (Enum.TryParse(modStr, true, out CardModifiers mod)) cardModifiers |= mod;
            else throw new Exception($"Couldn't parse invalid modifier [{modStr}] for card [{cardName}]");
        }

        GetReferences();

        UpdateObjectValues();

        gameObject.name = data.name;
    }

    public virtual void GetReferences()
    {
        TMP_Text GetText(string str) => transform.Find(str).GetComponent<TMP_Text>();
        Image GetImage(string str) => transform.Find(str).GetComponent<Image>();

        baseImage = GetImage("CardBase");
        artImage = GetImage("CardArt");
        nameField = GetText("CardName");
        descriptionField = GetText("CardDescription");
        leftField = GetText("CardFieldLeft");
        rightField = GetText("CardFieldRight");

        cardCollider = GetComponent<BoxCollider>();
        cardPositionManager = gameObject.AddComponent<CardPositionManager>();
    }

    public virtual void UpdateObjectValues()
    {
        baseImage.sprite = cardBase;
        artImage.sprite = cardArt;
        nameField.text = cardName;
        descriptionField.text = cardDescription;
    }
}

public class GunCardObject : CardObject
{
    [Header("Values")]
    public float damageValue;
    public int ammunition;

    public override void InitializeValues(CardData data)
    {
        base.InitializeValues(data);
        damageValue = data.damage;
        ammunition = data.ammo;
    }

    public override void UpdateObjectValues()
    {
        base.UpdateObjectValues();
        leftField.text = $"{cardLeftFieldText ??= "ATK:"} {damageValue}";
        rightField.text = $"{cardRightFieldText ??= "AMM:"} {ammunition}";
    }
}

public class ConsumableCardObject : CardObject
{
    // Ignore this, it's not finished
    public override void InitializeValues(CardData data)
    {
        base.InitializeValues(data);
    }
}