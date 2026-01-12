using UnityEngine;
using UnityEngine.InputSystem;
using System;


[RequireComponent(typeof(Player))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed;
    public float acceleration;

    [NonSerialized] public Vector2 moveInput;
    [NonSerialized] public Vector3 forward;
    [NonSerialized] public Vector3 right;
    [NonSerialized] public Vector3 moveDirection;

    [Header("Jump")]
    public float jumpForce;
    public Timer jumpCooldown;

    [Header("Dash")]
    public float dashForce;
    public Timer dashCooldown;

    [NonSerialized] public bool hasDash;

    public GrappleHandler grapple;

    [Header("Grounding")]
    public float groundCheckSphereRadius;
    private float groundCheckDistance;
    
    [Header("References")]
    private Player player;
    private Rigidbody rb;
    private Camera cam;
    private PlayerInputActions.MovementActions input;
    
    private void Awake()
    {
        player = GetComponent<Player>();
        rb = player.playerRigidbody;
        cam = player.playerCamera;
        input = player.input.Movement;

        groundCheckDistance = GetComponent<CapsuleCollider>().height / 2;
    }

    private void OnEnable()
    {
        SubscribeActions();
    }

    private void OnDisable()
    {
        UnsubscribeActions();
    }

    public bool IsGrounded()
    { 
        return Physics.CheckSphere(
            transform.position + Vector3.down * groundCheckDistance, 
            groundCheckSphereRadius,
            ~(1 << gameObject.layer) // Bitmask shenaningans, godfuckingdamnit
            );
    }

    private void FixedUpdate()
    {
        UpdateInput();
        TrackCameraYRotation();
        ProcessPlayerMovement();
        hasDash |= IsGrounded();
    }

    private void UpdateInput()
    {
        moveInput = input.Direction.ReadValue<Vector2>();
        forward = Vector3.Scale(cam.transform.forward, new(1, 0, 1)).normalized;
        right = Vector3.Scale(cam.transform.right, new(1, 0, 1)).normalized;
        moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;
    }

    private void TrackCameraYRotation()
    {
        transform.rotation = Quaternion.Euler(0f, cam.transform.eulerAngles.y, 0f);
    }

    private void ProcessPlayerMovement()
    {
        Vector3 targetVel = moveDirection * movementSpeed;
        
        Vector3 horizontalVel = new(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        
        Vector3 velocityDelta = targetVel - horizontalVel;

        rb.AddForce(velocityDelta * acceleration, ForceMode.VelocityChange);
    }

    private void TryJump(InputAction.CallbackContext ctx)
    {
        if (!IsGrounded() || !jumpCooldown.Ready) return;

        rb.linearVelocity = Vector3.Scale(rb.linearVelocity, new(1, 0, 1));
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        
        jumpCooldown.Start();
    }

    private void TryDash(InputAction.CallbackContext ctx)
    {
        if (!hasDash || !dashCooldown.Ready) return;

        hasDash = false;

        Vector3 dashDirection = moveDirection != Vector3.zero ? moveDirection : rb.transform.forward;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(dashDirection * dashForce, ForceMode.VelocityChange);

        dashCooldown.Start();
    }

    internal void SubscribeActions()
    {
        input.Jump.performed += TryJump;

        input.Dash.performed += TryDash;
    }

    internal void UnsubscribeActions()
    {
        input.Jump.performed -= TryJump;

        input.Dash.performed -= TryDash;
    }
}

[Serializable]
public class Timer
{
    public float duration;
    private float start;
    private float end;

    public Timer(float duration) => this.duration = duration;

    public bool Ready => Time.time >= end;

    public float Progress => Mathf.Clamp01(Mathf.InverseLerp(start, end, Time.time));

    public void Start()
    {
        start = Time.time;
        end = start + duration;
    }
}