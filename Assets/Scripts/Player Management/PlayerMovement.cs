using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed;
    public float acceleration;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;

    [Header("Dash")]
    public float dashForce;
    public float dashCooldown;
    public bool hasDash;

    [Header("Slam")]
    public float slamFallSpeed;

    [Header("Grounding")]
    public float groundCheckDistance;
    public float groundCheckSphereRadius;

    private Coroutine jumpCoroutine, dashCoroutine, slamCoroutine;
    private Player player;
    
    private void Awake()
    {
        player = GetComponent<Player>();
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

    internal void FixedUpdate()
    {
        TrackCameraYRotation();
        ProcessPlayerMovement();
        hasDash |= IsGrounded();
    }

    private void TrackCameraYRotation()
    {
        transform.rotation = Quaternion.Euler(0f, player.playerCamera.transform.eulerAngles.y, 0f);
    }

    private void ProcessPlayerMovement()
    {
        Vector2 movementInput = player.input.Player.Movement.ReadValue<Vector2>();
        Vector3 forward = player.playerCamera.transform.forward;
        Vector3 right = player.playerCamera.transform.right;
        forward.y = 0;
        right.y = 0;
        Vector3 moveDir = (forward * movementInput.y + right * movementInput.x).normalized;
        Vector3 targetVel = moveDir * movementSpeed;
        Rigidbody rb = player.playerRigidbody;
        Vector3 horizontalVel = new(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        Vector3 newHorizontalVel = Vector3.Lerp(horizontalVel, targetVel, acceleration);
        rb.linearVelocity = new Vector3(newHorizontalVel.x, rb.linearVelocity.y, newHorizontalVel.z);
    }

    internal void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        if (IsGrounded()) jumpCoroutine ??= player.StartCoroutine(Jump());
    }

    internal void OnDashPerformed(InputAction.CallbackContext ctx)
    {
        if (hasDash) dashCoroutine ??= player.StartCoroutine(Dash());
    }

    internal void OnSlamPerformed(InputAction.CallbackContext ctx)
    {
        if (!IsGrounded()) slamCoroutine ??= player.StartCoroutine(Slam());
    }

    internal IEnumerator Jump()
    {
        Rigidbody rb = player.playerRigidbody;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        yield return new WaitForSeconds(jumpCooldown);
        jumpCoroutine = null;
    }

    internal IEnumerator Dash()
    {
        hasDash = false;
        Rigidbody rb = player.playerRigidbody;
        Vector2 moveInput = player.input.Player.Movement.ReadValue<Vector2>();
        Vector3 forward = rb.transform.forward;
        Vector3 right = rb.transform.right;
        forward.y = 0;
        right.y = 0;
        Vector3 dashDir = (forward * moveInput.y + right * moveInput.x).normalized;
        if (dashDir == Vector3.zero) dashDir = rb.transform.forward;
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(dashDir * dashForce, ForceMode.Impulse);
        yield return new WaitForSeconds(dashCooldown);
        dashCoroutine = null;
    }

    internal IEnumerator Slam()
    {
        while (!IsGrounded())
        {
            if (player.input.Player.Dash.ReadValue<float>() > 0f)
            {
                slamCoroutine = null;
                yield break;
            }
            player.playerRigidbody.linearVelocity = Vector3.down * Time.fixedDeltaTime * slamFallSpeed;
            yield return null;
        }
        slamCoroutine = null;
    }

    internal void SubscribeActions()
    {
        player.input.Player.Jump.performed += OnJumpPerformed;
        player.input.Player.Dash.performed += OnDashPerformed;
        player.input.Player.Slam.performed += OnSlamPerformed;
    }

    internal void UnsubscribeActions()
    {
        player.input.Player.Jump.performed -= OnJumpPerformed;
        player.input.Player.Dash.performed -= OnDashPerformed;
        player.input.Player.Slam.performed -= OnSlamPerformed;
    }
}