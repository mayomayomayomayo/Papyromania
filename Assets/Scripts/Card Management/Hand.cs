using UnityEngine; 
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class Hand : MonoBehaviour
{
    [Header("Hand stats")]
    public int maxHandSize;

    [Header("Hand visuals")]
    public float handSpread;
    public float handRotation;
    public float handDrop;
    
    [Header("Hand")]
    public CardStructure cards = new();
    public Card currentCard;

    private Player player;

    private void Awake() => player = GetComponent<Player>();

    private void OnEnable() => SubscribeActions();

    private void OnDisable() => UnsubscribeActions();
    
    internal void OnUsePerformed(InputAction.CallbackContext ctx)
    {
        if (currentCard == null) return;
        if ((currentCard.data.modifiers & (CardDefinition.CardModifiers.NonUsable | CardDefinition.CardModifiers.NonUsablePrimary)) == 0) 
        currentCard.behaviour.UsePrimary();
    }

    internal void OnSecondaryUsePerformed(InputAction.CallbackContext ctx)
    {
        if (currentCard == null) return;
        if ((currentCard.data.modifiers & (CardDefinition.CardModifiers.NonUsable | CardDefinition.CardModifiers.NonUsableSecondary)) == 0)
        currentCard.behaviour.UseSecondary();
    }

    internal void OnDiscardPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log($"discarded {(currentCard != null ? currentCard.name : "something probaby")}");
    }
    
    internal void OnCardAdded(Card card)
    {
        if (currentCard == null) currentCard = card;
        UpdateHandLayout();
    }

    internal void SubscribeActions()
    {
        player.input.Player.Use.performed += OnUsePerformed;
        player.input.Player.SecondaryUse.performed += OnSecondaryUsePerformed;
        player.input.Player.Discard.performed += OnDiscardPerformed;
        cards.onAddCard += OnCardAdded;
    }

    internal void UnsubscribeActions()
    {
        player.input.Player.Use.performed -= OnUsePerformed;
        player.input.Player.SecondaryUse.performed -= OnSecondaryUsePerformed;
        player.input.Player.Discard.performed -= OnDiscardPerformed;
        cards.onAddCard -= OnCardAdded;
    }

    public void UpdateHandLayout()
    {
        foreach (Card card in cards.cards)
        {
            if (card == currentCard) continue;

            int iDelta = cards.FindIndex(card) - cards.FindIndex(currentCard);
            float sqrtIDelta = Mathf.Sqrt(iDelta);

            card.transform.SetLocalPositionAndRotation(
                Vector3.left * sqrtIDelta * handSpread + Vector3.down * sqrtIDelta * handDrop, 
                Quaternion.Euler(0f, 0f, iDelta * handRotation)
                );
        }
    }
}