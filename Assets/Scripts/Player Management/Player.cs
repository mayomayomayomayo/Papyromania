using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Welcome to hell

public sealed class Player : MonoBehaviour
{
    public References references;
    public Movement movement;
    public Hand hand;

    public static PlayerInputActions input;

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
        movement.EnableMovementActions();
    }

    private void OnDisable()
    {
        input.Player.Disable();
        movement.DisableMovementActions();
    }

    [Serializable]
    public class References
    {
        public GameObject player;

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

        public bool IsGrounded => 
            Physics.CheckSphere(
                player.transform.position + Vector3.down * groundCheckDistance, 
                groundCheckSphereRadius,
                ~(1 << player.gameObject.layer) // Bitmask shenaningans, godfuckingdamnit
            );

        internal void FixedUpdate()
        {
            TrackCameraYRotation();
            ProcessPlayerMovement();

            hasDash |= IsGrounded;
        }

        private void TrackCameraYRotation()
        {
            player.transform.rotation = Quaternion.Euler(0f, player.references.playerCamera.transform.eulerAngles.y, 0f);
        }

        private void ProcessPlayerMovement()
        {
            Vector2 movementInput = input.Player.Movement.ReadValue<Vector2>();

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
            if (IsGrounded) jumpCoroutine ??= player.StartCoroutine(Jump());
        }

        internal void OnDashPerformed(InputAction.CallbackContext ctx)
        {
            if (hasDash) dashCoroutine ??= player.StartCoroutine(Dash());
        }

        internal void OnSlamPerformed(InputAction.CallbackContext ctx)
        {
            if (!IsGrounded) slamCoroutine ??= player.StartCoroutine(Slam());
        }

        internal IEnumerator Jump()
        {
            Rigidbody rb = player.references.playerRigidbody;

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            yield return new WaitForSeconds(jumpCooldown);
            jumpCoroutine = null;
        }

        internal IEnumerator Dash()
        {
            hasDash = false;

            Rigidbody rb = player.references.playerRigidbody;

            Vector2 moveInput = input.Player.Movement.ReadValue<Vector2>();
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
            while (!IsGrounded)
            {
                if (input.Player.Dash.ReadValue<float>() > 0f)
                {
                    slamCoroutine = null;
                    yield break;
                }

                player.references.playerRigidbody.linearVelocity = Vector3.down * Time.fixedDeltaTime * slamFallSpeed;
                yield return null;
            }

            slamCoroutine = null;
        }

        internal void EnableMovementActions()
        {
            input.Player.Jump.performed += OnJumpPerformed;
            input.Player.Dash.performed += OnDashPerformed;
            input.Player.Slam.performed += OnSlamPerformed;
        }

        internal void DisableMovementActions()
        {
            input.Player.Jump.performed -= OnJumpPerformed;
            input.Player.Dash.performed -= OnDashPerformed;
            input.Player.Slam.performed -= OnSlamPerformed;
        }
    }

    [Serializable]
    public class Hand
    {
        [Header("Hand stats")]
        public int maxHandSize;
        
        [Header("Hand")]
        public List<CardObject> handList;
    }
}