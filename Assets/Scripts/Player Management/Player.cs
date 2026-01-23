using UnityEngine;

public sealed class Player : MonoBehaviour
{
    [Header("References")]
    public Transform playerHandAnchor;
    public Camera playerCamera;
    public Rigidbody playerRigidbody;
    public Hand playerHand;
    public PlayerMovement playerMovement;
    public InputContext inputContext;
    public CapsuleCollider playerCollider;

    public PlayerStats stats;

    [Header("Movement")]
    public PlayerMovement movement;
}