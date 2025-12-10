using System;
using UnityEngine;
using System.Collections;

public static class HandAnimations
{
    public enum HandLayout
    {
        Resting,
        Aiming
    }

    public static void UpdateLayout(this Hand hand, HandLayout layout)
    {
        foreach (Coroutine cr in hand.visuals.coroutines) if (cr != null) hand.StopCoroutine(cr);
        hand.visuals.coroutines.Clear();
        
        UpdateLayoutInternal(hand, layout);
    }

    private static void UpdateLayoutInternal(Hand hand, HandLayout layout)
    {
        int currentCardIndex = hand.handStructure.FindIndex(hand.currentCard);

        foreach (Card card in hand.handStructure.Cards)
        {
            int iDelta = hand.handStructure.FindIndex(card) - currentCardIndex;
            float sqrtIDelta = Mathf.Sqrt(Mathf.Abs(iDelta));

            PosRot offset = layout switch
            {
                HandLayout.Resting => new(
                    -sqrtIDelta * hand.visuals.handSpreadResting * Mathf.Sign(iDelta),
                    -sqrtIDelta * hand.visuals.handDrop,
                    0.05f * Mathf.Abs(iDelta),

                    Quaternion.Euler(0f, 0f, iDelta * hand.visuals.handRotation)
                ),

                HandLayout.Aiming => new(
                    sqrtIDelta * hand.visuals.handSpreadAiming * Mathf.Sign(iDelta),
                    0f,
                    0.05f * Math.Abs(iDelta),

                    Quaternion.identity
                ),

                _ => throw new Exception("physically impossible btw")
            };

            hand.visuals.coroutines.Add(hand.StartCoroutine(OrderCard(
                card,
                offset,
                hand.visuals.cardOrderSpeed
            )));
        }

        static IEnumerator OrderCard(Card card, PosRot target, float step, float snap = 0.001f)
        {
            while (Vector3.Distance(card.transform.localPosition, target.position) >= snap)
            {
                card.transform.SetLocalPositionAndRotation(
                    Vector3.Lerp(card.transform.localPosition, target.position, step * Time.deltaTime),
                    Quaternion.Slerp(card.transform.localRotation, target.rotation, step * Time.deltaTime)
                );

                yield return null;
            }

            card.transform.SetLocalPositionAndRotation(target.position, target.rotation);
        }
    }
}

[Serializable]
public struct PosRot
{
    public Vector3 position;
    public Quaternion rotation;

    public PosRot(float x, float y, float z, Quaternion rot)
    {
        position = new(x, y, z);
        rotation = rot;
    }
}