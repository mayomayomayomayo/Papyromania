using UnityEngine;

[RequireComponent(typeof(Player))]
[DefaultExecutionOrder(-30)]
public sealed class MovementContext : MonoBehaviour
{
    private Player p;

    public PlayerInputActions input;
    
    public Vector2 MoveInput { get; private set; }
    public Vector3 Forward { get; private set; }
    public Vector3 Right { get; private set; }
    public Vector3 Forward2D { get; private set; }
    public Vector3 Right2D { get; private set; }
    public Vector3 MoveDirection { get; private set; }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(
            transform.position + Vector3.down * (p.physical.collider.height / 2),
            p.physical.collider.radius / 2,
            ~(1 << gameObject.layer)
        );
    }

    public bool IsBumpingWall(out Vector3 wallNormal)
    {
        bool wallHit = Physics.Raycast(
            transform.position, 
            MoveDirection, 
            out RaycastHit rh,
            p.stats.wallCheckDistance
            ) && rh.normal.y < GameConstants.MIN_WALL_NORMAL_Y;

        wallNormal = wallHit ? rh.normal : Vector3.zero;
        return wallHit;
    }

    public bool IsHuggingWall(out Vector3 wallNormal)
    {
        float side = MoveInput.x;
        
        if (side == 0f) // Cause zero-length vectors piss off Physics.Raycast
        {
            wallNormal = Vector3.zero;
            return false;
        }

        bool isHugging = Physics.Raycast(
            transform.position,
            p.physical.cam.transform.right * Mathf.Sign(side),
            out RaycastHit hit,
            p.stats.wallCheckDistance
        );

        wallNormal = hit.normal;

        return isHugging;
    }

    private void FixedUpdate()
    {
        MoveInput = input.Movement.Direction.ReadValue<Vector2>();

        Transform camTransform = p.physical.cam.transform;

        Forward = camTransform.forward.normalized;
        Right = camTransform.right.normalized;

        Forward2D = new Vector3(Forward.x, 0, Forward.z).normalized;
        Right2D = new Vector3(Right.x, 0, Right.z).normalized;
        
        MoveDirection = (Forward2D * MoveInput.y + Right2D * MoveInput.x).normalized;
    }

    private void Awake()
    {
        p = GetComponent<Player>();
        input = new();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}