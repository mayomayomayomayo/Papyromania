using System;
using UnityEngine; 
using UnityEngine.InputSystem;

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
    
    internal void OnUsePerformed(InputAction.CallbackContext ctx)
    {
        if (currentCard == null) return;
        if ((currentCard.definition.modifiers & (CardDefinition.CardModifiers.NonUsable | CardDefinition.CardModifiers.NonUsablePrimary)) == 0) 
        currentCard.behaviour.UsePrimary();
    }

    internal void OnSecondaryUsePerformed(InputAction.CallbackContext ctx)
    {
        if (currentCard == null) return;
        if ((currentCard.definition.modifiers & (CardDefinition.CardModifiers.NonUsable | CardDefinition.CardModifiers.NonUsableSecondary)) == 0)
        currentCard.behaviour.UseSecondary();
    }

    internal void OnDiscardPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log($"discarded {(currentCard != null ? currentCard.name : "something probaby")}");
    }

    internal void OnPreviousCardPerformed(InputAction.CallbackContext ctx)
    {
        int index = handStructure.FindIndex(currentCard) - 1;
        
        currentCard = index >= 0 ? 
            handStructure.Cards[index] :
            handStructure.Cards[^1];

        onHandUpdated?.Invoke();
    }

    internal void OnNextCardPerformed(InputAction.CallbackContext ctx)
    {
        int index = handStructure.FindIndex(currentCard) + 1;

        currentCard = index < handStructure.Cards.Count ? 
            handStructure.Cards[index] :
            handStructure.Cards[0];

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
        player.input.Player.Use.performed += OnUsePerformed;
        player.input.Player.SecondaryUse.performed += OnSecondaryUsePerformed;
        player.input.Player.Discard.performed += OnDiscardPerformed;
        player.input.Player.PreviousCard.performed += OnPreviousCardPerformed;
        player.input.Player.NextCard.performed += OnNextCardPerformed;

        // Updates
        handStructure.onAddCard += OnCardAdded;
        handStructure.onRemoveCard += OnCardRemoved;

        // Visuals
        onHandUpdated += () => this.UpdateLayout(HandAnimations.HandLayout.Aiming);
    }

    internal void UnsubscribeActions()
    {
        player.input.Player.Use.performed -= OnUsePerformed;
        player.input.Player.SecondaryUse.performed -= OnSecondaryUsePerformed;
        player.input.Player.Discard.performed -= OnDiscardPerformed;
        player.input.Player.PreviousCard.performed -= OnPreviousCardPerformed;
        player.input.Player.NextCard.performed -= OnNextCardPerformed;

        // Updates
        handStructure.onAddCard -= OnCardAdded;
        handStructure.onRemoveCard -= OnCardRemoved;

        // Visuals
        onHandUpdated -= () => this.UpdateLayout(HandAnimations.HandLayout.Resting);
    }
}