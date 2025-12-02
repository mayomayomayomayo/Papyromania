using UnityEngine; 
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class Hand : MonoBehaviour
{
    [Header("Hand stats")]
    public int maxHandSize;
    
    [Header("Hand")]
    public CardStructure hand = new();
    public CardObject currentCard;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        SubscribeActions();
    }

    private void OnDisable()
    {
        UnsubscribeActions();
    }
    
    internal void OnUsePerformed(InputAction.CallbackContext ctx)
    {
        if (currentCard == null) return;
        if ((currentCard.cardModifiers & (CardObject.CardModifiers.NonUsable | CardObject.CardModifiers.NonUsablePrimary)) == 0) 
        currentCard.OnPlay();
    }

    internal void OnSecondaryUsePerformed(InputAction.CallbackContext ctx)
    {
        if (currentCard == null) return;
        if ((currentCard.cardModifiers & (CardObject.CardModifiers.NonUsable | CardObject.CardModifiers.NonUsableSecondary)) == 0)
        currentCard.OnSecondaryPlay();
    }

    internal void OnDiscardPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log($"discarded {(currentCard != null ? currentCard.cardName : "something probaby")}");
    }
    
    internal void SubscribeActions()
    {
        player.input.Player.Use.performed += OnUsePerformed;
        player.input.Player.SecondaryUse.performed += OnSecondaryUsePerformed;
        player.input.Player.Discard.performed += OnDiscardPerformed;
    }

    internal void UnsubscribeActions()
    {
        player.input.Player.Use.performed -= OnUsePerformed;
        player.input.Player.SecondaryUse.performed -= OnSecondaryUsePerformed;
        player.input.Player.Discard.performed -= OnDiscardPerformed;
    }

    internal void UpdateHandLayout()
    {
        foreach (CardObject card in hand.cards)
        {
            if (card == currentCard) continue;
            Debug.Log($"Card examined: {card.cardName}"); // TODO implement this
        }
    }
}