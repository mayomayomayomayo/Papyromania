using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Card))]
public class CardPositionManager : MonoBehaviour
{
    public Card obj;

    private Transform handAnchor;
    private Player player;

    private void Awake() => obj = GetComponent<Card>();

    public void OnPickup(GameObject plr)
    {
        player = plr.GetComponent<Player>();
        handAnchor = player.playerHandAnchor.transform;

        StartCoroutine(GoToHand(transform, handAnchor));
    }

    public IEnumerator GoToHand(Transform obj, Transform target, float moveSpeed = 0.1f, float snapDistance = 0.01f)
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

        obj.SetPositionAndRotation(target.position, target.rotation);
        player.hand.UpdateHandLayout();
    }
}