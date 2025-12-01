using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CardObject))]
public class CardPositionManager : MonoBehaviour
{
    [Header("References")]
    public CardObject obj;

    [Header("Private references")]
    private Coroutine movingCr;
    private Transform playerHandAnchor;

    private void Awake()
    {
        obj = GetComponent<CardObject>();
    }

    public void OnPickup(GameObject player)
    {
        // uNItY oBJEctS shOuLd NOt uSE nUll CoAlEscIng
        playerHandAnchor = playerHandAnchor != null ? playerHandAnchor : player.GetComponent<Player>().playerHandAnchor.transform;

        this.KillCoroutine(ref movingCr);
        StartCoroutine(GoToAndParent(transform, playerHandAnchor));
        
    }

    public IEnumerator GoToAndParent(Transform obj, Transform target, float moveSpeed = 0.1f, float snapDistance = 0.01f)
    {
        obj.SetParent(target);

        while (Vector3.Distance(obj.position, target.position) >= snapDistance)
        {
            obj.SetPositionAndRotation(
                Vector3.Lerp(obj.position, target.position, moveSpeed),
                Quaternion.Slerp(obj.rotation, target.rotation, moveSpeed)
            );
            yield return null;
        }

        // Btw i still find SetPositionAndRotation funny cause why do we need a method for that
        obj.SetPositionAndRotation(target.position, target.rotation);
        movingCr = null;
    }
}