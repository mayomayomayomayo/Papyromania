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

    public static void UpdateLayout(this Hand hand, HandLayout layout) // THE ARCANE METH (has been contained)
    {
        foreach (Coroutine cr in hand.visuals.coroutines) if (cr != null) hand.StopCoroutine(cr);
        hand.visuals.coroutines.Clear();

        int currentCardIndex = hand.handStructure.FindIndex(hand.currentCard);

        for (int i = 0; i < hand.handStructure.Count; i++)
        {
            Card card = hand.handStructure[i];
            HandVisuals vis = hand.visuals;
            int handSize = hand.handStructure.Count;
            
            int iDelta = i - currentCardIndex;
            int maxDelta = Mathf.Max(currentCardIndex, handSize - 1 - currentCardIndex);

            float t = maxDelta > 0 ? (float) iDelta / maxDelta : 0f;

            TransformData offset = layout switch
            {
                HandLayout.Resting => ProcessOffset(vis.restingLayout, t),

                HandLayout.Aiming => ProcessOffset(vis.aimingLayout, t),
            
                _ => throw new Exception("how")
            };

            hand.visuals.coroutines.Add(hand.StartCoroutine(UpdateCardOrder(
                card,
                offset,
                vis.cardOrderSpeed
            )));
        }

        static IEnumerator UpdateCardOrder(Card card, TransformData target, float step, float snap = 0.001f)
        {
            while (Vector3.Distance(card.transform.localPosition, target.position) >= snap)
            {
                card.transform.SetLocalPositionAndRotation(
                    Vector3.Lerp(card.transform.localPosition, target.position, step * Time.deltaTime),
                    Quaternion.Slerp(card.transform.localRotation, target.rotation, step * Time.deltaTime)
                );
                card.transform.localScale = Vector3.Lerp(card.transform.localScale, target.scale, step * Time.deltaTime);

                yield return null;
            }

            card.transform.SetLocalPositionAndRotation(target.position, target.rotation);
            card.transform.localScale = target.scale;
        }

        static TransformData ProcessOffset(HandLayoutData ld, float t)
        {
            return new(
                ProcessValues(ld.rawOffset.position, ld.positionCurves, t),
                Quaternion.Euler(ProcessValues(ld.rawOffset.rotation.eulerAngles, ld.rotationCurves, t)),
                ProcessValues(ld.rawOffset.scale, ld.scaleCurves, t)
            );

            static Vector3 ProcessValues(Vector3 raw, AxisCurves curves, float t)
            {
                return new(
                    raw.x * curves.x.Evaluate(t),
                    raw.y * curves.y.Evaluate(t),
                    raw.z * curves.z.Evaluate(t)
                );
            }
        }
    }
}

[Serializable]
public struct TransformData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public TransformData(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }
}