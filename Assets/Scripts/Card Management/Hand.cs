using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.InputSystem;

// It's gonna be REALLY funny when i have to refactor this one (edit: which i 100% will very much have to)
// Fuck you, future Mayo
// Hehehehehe
// You're gonna kill yourself yes you are
// HEHEHEHEHEHHEHE

[RequireComponent(typeof(Player))]
public class Hand : MonoBehaviour
{
    [Header("Hand stats")]
    public int maxHandSize;

    [Header("Hand visuals")]
    public float handSpread;
    public float handRotation;
    public float handDrop;
    public float cardOrderSpeed;
    public List<Coroutine> movingCoroutines;
    
    [Header("Hand")]
    public CardStructure cards = new();
    public Card currentCard;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
        movingCoroutines = new();
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
        int index = cards.FindIndex(currentCard) - 1;

        currentCard = index >= 0 ? 
            cards.cards[index] :
            cards.cards[^1];

        UpdateHandLayout();
    }

    internal void OnNextCardPerformed(InputAction.CallbackContext ctx)
    {
        int index = cards.FindIndex(currentCard) + 1;

        currentCard = index < cards.cards.Count ? 
            cards.cards[index] :
            cards.cards[0];

        UpdateHandLayout();
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
        player.input.Player.PreviousCard.performed += OnPreviousCardPerformed;
        player.input.Player.NextCard.performed += OnNextCardPerformed;
        cards.onAddCard += OnCardAdded;
    }

    internal void UnsubscribeActions()
    {
        player.input.Player.Use.performed -= OnUsePerformed;
        player.input.Player.SecondaryUse.performed -= OnSecondaryUsePerformed;
        player.input.Player.Discard.performed -= OnDiscardPerformed;
        player.input.Player.PreviousCard.performed -= OnPreviousCardPerformed;
        player.input.Player.NextCard.performed -= OnNextCardPerformed;
        cards.onAddCard -= OnCardAdded;
    }

    public void UpdateHandLayout()
    {
        int currentCardIndex = cards.FindIndex(currentCard);

        foreach (Coroutine cr in movingCoroutines) 
        {
            if (cr != null) StopCoroutine(cr);
        }
        movingCoroutines.Clear();

        foreach (Card card in cards.cards)
        {
            int iDelta = cards.FindIndex(card) - currentCardIndex;
            float sqrtIDelta = Mathf.Sqrt(Mathf.Abs(iDelta));

            float yOffset = sqrtIDelta * handSpread * Mathf.Sign(iDelta);
            float xOffset = sqrtIDelta * handDrop;

            movingCoroutines.Add(StartCoroutine(MoveCardToOrder(
                card,
                Vector3.left * yOffset + Vector3.down * xOffset + Vector3.forward * iDelta * 0.001f,
                Quaternion.Euler(0f, 0f, iDelta * handRotation),
                cardOrderSpeed
                )));
        }

        static IEnumerator MoveCardToOrder(Card card, Vector3 targetPos, Quaternion targetRot, float step = 1f, float snapDistance = 0.01f)
        {
            while (Vector3.Distance(card.transform.localPosition, targetPos) >= snapDistance)
            {
                card.transform.SetLocalPositionAndRotation(
                    Vector3.Lerp(card.transform.localPosition, targetPos, step * Time.deltaTime), 
                    Quaternion.Slerp(card.transform.localRotation, targetRot, step * Time.deltaTime)
                );
                yield return null;
            }
        }
    }
}