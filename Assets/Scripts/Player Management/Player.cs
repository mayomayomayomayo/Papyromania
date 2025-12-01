using UnityEngine;

public sealed class Player : MonoBehaviour
{
    [Header("References")]
    public GameObject playerHandAnchor;
    public Camera playerCamera;
    public Rigidbody playerRigidbody;
    public Hand hand;
    public PlayerInputActions input;

    private void Awake()
    {
        input = new();
    }

    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }
}