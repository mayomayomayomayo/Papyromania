using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// Welcome to hell

[RequireComponent(typeof(MBRunner))]
public sealed class Player : MonoBehaviour
{
    public References references;
    public Movement movement;
    public MBRunner runner;

    public PlayerInputActions input;


    private void Awake()
    {
        input = new();
        movement.Init(this);
    }

    private void FixedUpdate()
    {
        movement.FixedUpdate();
    }

    private void OnEnable()
    {
        input.Player.Enable();
        EnableMovementActions();
    }

    private void OnDisable()
    {
        input.Player.Disable();
        DisableMovementActions();
    }

    private void EnableMovementActions()
    {
        input.Player.Jump.performed += movement.OnJumpPerformed;
        input.Player.Dash.performed += movement.OnDashPerformed;
        input.Player.Slam.performed += movement.OnSlamPerformed;
    }

    private void DisableMovementActions()
    {
        input.Player.Jump.performed -= movement.OnJumpPerformed;
        input.Player.Dash.performed -= movement.OnDashPerformed;
        input.Player.Slam.performed -= movement.OnSlamPerformed;
    }

    [Serializable]
    public class References
    {
        public GameObject playerBase;

        public GameObject playerHitbox;

        public GameObject playerHandAnchor;

        public Camera playerCamera;

        public Rigidbody playerRigidbody;
    }

    [Serializable]
    public class Movement
    {
        private Player player;
        public void Init(Player p) => player = p;


        [Header("Movement")]
        public float movementSpeed;
        public float acceleration;

        [Header("Jump")]
        public float jumpForce;
        public float jumpCooldown;

        [Header("Dash")]
        public float dashForce;
        public float dashCooldown;

        [Header("Slam")]
        public float slamFallSpeed;

        [Header("Grounding")]
        public float groundCheckDistance;
        public float groundCheckSphereRadius;

        private Coroutine jumpCoroutine, dashCoroutine, slamCoroutine;
        private bool hasDash;

        internal void FixedUpdate()
        {
            TrackCameraYRotation();
            ProcessPlayerMovement();

            hasDash |= IsGrounded;
        }

        private void TrackCameraYRotation()
        {
            player.references.playerHitbox.transform.rotation = Quaternion.Euler(0f, player.references.playerCamera.transform.eulerAngles.y, 0f);
        }

        private void ProcessPlayerMovement()
        {
            Vector2 movementInput = player.input.Player.Movement.ReadValue<Vector2>();

            Vector3 forward = player.references.playerCamera.transform.forward;
            Vector3 right = player.references.playerCamera.transform.right;

            forward.y = 0;
            right.y = 0;

            Vector3 moveDir = (forward * movementInput.y + right * movementInput.x).normalized;
            Vector3 targetVel = moveDir * movementSpeed;

            Rigidbody rb = player.references.playerRigidbody;

            Vector3 horizontalVel = new(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            Vector3 newHorizontalVel = Vector3.Lerp(horizontalVel, targetVel, acceleration);

            rb.linearVelocity = new Vector3(newHorizontalVel.x, rb.linearVelocity.y, newHorizontalVel.z);
        }

        internal void OnJumpPerformed(InputAction.CallbackContext ctx)
        {
            if (IsGrounded) jumpCoroutine ??= player.runner.StartCoroutine(Jump());
        }

        internal void OnDashPerformed(InputAction.CallbackContext ctx)
        {
            if (hasDash) dashCoroutine ??= player.runner.StartCoroutine(Dash());
        }

        internal void OnSlamPerformed(InputAction.CallbackContext ctx)
        {
            if (!IsGrounded) slamCoroutine ??= player.runner.StartCoroutine(Slam());
        }

        public IEnumerator Jump()
        {
            Rigidbody rb = player.references.playerRigidbody;

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            yield return new WaitForSeconds(jumpCooldown);
            jumpCoroutine = null;
        }

        public IEnumerator Dash()
        {
            hasDash = false;

            Rigidbody rb = player.references.playerRigidbody;

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

        public IEnumerator Slam()
        {
            Debug.Log("Slammed");
            yield return null;
        }

        public bool IsGrounded => Physics.CheckSphere(player.references.playerHitbox.transform.position + Vector3.down * groundCheckDistance, groundCheckSphereRadius);
    }
}
