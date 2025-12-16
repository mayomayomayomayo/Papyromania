using System;
using UnityEngine; 
using UnityEngine.InputSystem;
using System.Collections.Generic;

[RequireComponent(typeof(Player))]
public class Hand : MonoBehaviour
{
    [Header("Divisions")]
    public HandVisuals visuals;
    
    [Header("Hand")]
    public CardStructure handStructure = new();
    public int maxHandSize;
    public Card currentCard;

    [Header("Events")]
    public Action onHandUpdated;
    public static List<InputAction> handUpdatingActions;

    private Player player;

    private void Awake()
    {
        GetRefs(); 
        visuals.hand = this;
    }

    private void GetRefs()
    {
        player = GetComponent<Player>();
    }

    private void OnEnable() => SubscribeActions();

    private void OnDisable() => UnsubscribeActions();
    
    internal void Use()
    {
        if (currentCard == null) return;
        if ((currentCard.definition.modifiers & (CardDefinition.CardModifiers.NonUsable | CardDefinition.CardModifiers.NonUsablePrimary)) == 0) 
        currentCard.behaviour.UsePrimary();
    }

    internal void SecondaryUse()
    {
        if (currentCard == null) return;
        if ((currentCard.definition.modifiers & (CardDefinition.CardModifiers.NonUsable | CardDefinition.CardModifiers.NonUsableSecondary)) == 0)
        currentCard.behaviour.UseSecondary();
    }

    internal void Discard()
    {
        Debug.Log($"discarded {(currentCard != null ? currentCard.name : "something probaby")}");
    }

    public void PreviousCard()
    {
        int index = handStructure.FindIndex(currentCard) - 1;
        
        currentCard = index >= 0 ? 
            handStructure[index] :
            handStructure[^1];

        onHandUpdated?.Invoke();
    }

    public void NextCard()
    {
        int index = handStructure.FindIndex(currentCard) + 1;

        currentCard = index < handStructure.Count ? 
            handStructure[index] :
            handStructure[0];

        onHandUpdated?.Invoke();
    }
    
    internal void OnCardAdded(Card card)
    {
        if (currentCard == null) currentCard = card;
        onHandUpdated?.Invoke();
    }

    internal void OnCardRemoved(Card card)
    {
        onHandUpdated?.Invoke();
        throw new NotImplementedException("Not implemented because i've literally yet to implement this, not because you have to implement it yourself");
    }

    internal void SubscribeActions()
    {
        PlayerInputActions.CardUsageActions cardUsageMap = player.input.CardUsage; 

        cardUsageMap.Use.performed += _ => Use();
        cardUsageMap.SecondaryUse.performed += _ => SecondaryUse();
        cardUsageMap.Discard.performed += _ => Discard();
        cardUsageMap.PreviousCard.performed += _ => PreviousCard();
        cardUsageMap.NextCard.performed += _ => NextCard();

        // Updates
        handStructure.onAddCard += OnCardAdded;
        handStructure.onRemoveCard += OnCardRemoved;

        // Visuals
        onHandUpdated += () => this.UpdateLayout(HandAnimations.HandLayout.Resting);
    }

    internal void UnsubscribeActions()
    {
        PlayerInputActions.CardUsageActions cardUsageMap = player.input.CardUsage;
        
        cardUsageMap.Use.performed -= _ => Use();
        cardUsageMap.SecondaryUse.performed -= _ => SecondaryUse();
        cardUsageMap.Discard.performed -= _ => Discard();
        cardUsageMap.PreviousCard.performed -= _ => PreviousCard();
        cardUsageMap.NextCard.performed -= _ => NextCard();

        // Updates
        handStructure.onAddCard -= OnCardAdded;
        handStructure.onRemoveCard -= OnCardRemoved;

        // Visuals
        onHandUpdated -= () => this.UpdateLayout(HandAnimations.HandLayout.Resting);
    }
}