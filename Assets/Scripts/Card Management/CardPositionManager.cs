using UnityEngine;
using System.Collections;

public class CardPositionManager : MonoBehaviour
{
    public CardObject obj;

    private Coroutine movingCr;
    private Transform playerHandAnchor;

    public void OnPickup(GameObject playerHitbox)
    {
        // uNItY oBJEctS shOuLd NOt uSE nUll CoAlEscIng
        playerHandAnchor = playerHandAnchor != null ? playerHandAnchor : playerHitbox.GetComponent<Player>().references.playerHandAnchor.transform;

        this.KillCoroutine(ref movingCr);
        StartCoroutine(Move(transform, playerHandAnchor));
    }

    public IEnumerator Move(Transform obj, Transform target, float moveSpeed = 1f, float snapDistance = 0.01f)
    {
        movingCr = null;
        while (Vector3.Distance(obj.position, target.position) >= snapDistance)
        {
            obj.position = Vector3.Lerp(obj.position, target.position, moveSpeed);
            yield return null;
        }

        obj.position = target.position;
    }
}