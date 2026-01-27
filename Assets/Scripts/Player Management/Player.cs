using UnityEngine;

public sealed class Player : MonoBehaviour
{
    [Header("Refs")]
    public Transform playerHandAnchor;
    public Camera playerCamera;
    public Rigidbody playerRigidbody;
    public Hand playerHand;
    public InputContext playerInputCtx;
    public CapsuleCollider playerCollider;

    public PlayerStats stats;

    [Header("Movement")]
    public MovementContext playerMovementCtx;
    public PlayerMovement playerMovementManager;
    public JumpManager playerJumpManager;
    public DashManager playerDashManager;
}