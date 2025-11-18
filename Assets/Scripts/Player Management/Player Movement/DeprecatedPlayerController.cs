// using UnityEngine;
// using UnityEngine.InputSystem;
// using System.Collections;
// using UnityEngine.Rendering;
// using UnityEngine.Rendering.Universal;
// 
// // I'll probably remake this in the future cause it kinda sucks massive balls
// // Like why is the dash effect here :/
// // Read between the lines will ya, retard?? MOVE IT TO A CAMERAEFFECTS CLASS OR SOMETHING
// // Schizocommenting should be a real term
// 
// // IT'S TIME.
// 
// public class PlayerController : MonoBehaviour
// {
//     [Header("Camera/Mouse")]
//     // public bool cameraControllable = true;
//     // public bool invertMouseMovement = false;
//     public float mouseSensitivity = 30f; // todo make this actually do something
// 
//     [Header("Movement")]
//     public float movementSpeed = 1000f;
//     public float acceleration = 10f;
// 
//     [Header("Jumping")]
//     public float jumpForce = 10f;
//     public float jumpCooldown = 0.1f;
// 
//     [Header("Dashing")]
//     public float dashForce = 10f;
//     public float dashCooldown = 0.5f;
//     public bool hasDash;
//     public float dashFOVIncrease = 15f;
// 
//     [Header("Slamming")]
//     public float slamFallSpeed = 1000f;
// 
//     [Header("Input")]
//     public PlayerInputActions input;
// 
//     [Header("References")]
//     public Camera cam;
//     public Rigidbody rb;
//     public Transform groundCheckSphere;
//     public Coroutine dashCoroutine, jumpCoroutine, slamCoroutine;
//     public Volume postVolume;
// 
//     private MotionBlur motionBlur;
//     private ChromaticAberration chromAberration;
//     private Vignette vignette;
// 
//     private void Awake() //CHECKED
//     {
//         // DONE
//         input = new();
//         rb = transform.Find("Hitbox").GetComponent<Rigidbody>();
//         groundCheckSphere = transform.Find("Hitbox/GroundChecker");
// 
//         // Volume effects // HANDLE THIS IN CAMERAEFFECTS
//         postVolume.profile.TryGet(out motionBlur);
//         postVolume.profile.TryGet(out chromAberration);
//         postVolume.profile.TryGet(out vignette);
// 
//         CachePlayerReferences(); // INSPECTOR YA TWAT
//     }
// 
//     private void CachePlayerReferences() // Do you like hurting people?
//     {
//         // Cache references for easy access later
//         PlayerReferences.playerReference = gameObject;
//         PlayerReferences.playerPhysical = rb.gameObject;
//         PlayerReferences.playerCamera = cam;
//         PlayerReferences.playerHandAnchor = cam.transform.Find("HandAnchor").transform;
//         PlayerReferences.playerHand = GetComponent<Hand>(); // Give me patience
//     }
// 
//     private void OnEnable() // stupid
//     {
//         LockCursor();
//         EnableKeybinds();
//     }
// 
//     private void FixedUpdate() // Done except for IsGrounded
//     {
//         ProcessPlayerMovement();
//         TrackCamera();
//         hasDash |= IsGrounded();
//     }
// 
//     private void TrackCamera()
//     {
//         rb.transform.rotation = Quaternion.Euler(0f, cam.transform.eulerAngles.y, 0f);
//     }
// 
//     private void ProcessPlayerMovement()
//     {
//         Vector2 moveInput = input.Player.Movement.ReadValue<Vector2>();
// 
//         Vector3 forward = cam.transform.forward;
//         Vector3 right = cam.transform.right;
// 
//         forward.y = 0;
//         right.y = 0;
// 
//         Vector3 moveDir = (forward * moveInput.y + right * moveInput.x).normalized;
//         Vector3 targetVel = moveDir * movementSpeed;
// 
//         Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
// 
//         Vector3 newHorizontalVel = Vector3.Lerp(horizontalVel, targetVel, acceleration);
// 
//         rb.linearVelocity = new Vector3(newHorizontalVel.x, rb.linearVelocity.y, newHorizontalVel.z);
//     }
// 
//     private void OnJumpPerformed(InputAction.CallbackContext ctx)
//     {
//         if (IsGrounded()) jumpCoroutine ??= StartCoroutine(Jump());
//     }
// 
//     private void OnDashPerformed(InputAction.CallbackContext ctx)
//     {
//         if (hasDash) dashCoroutine ??= StartCoroutine(Dash());
//     }
// 
//     private void OnSlamPerformed(InputAction.CallbackContext ctx)
//     {
//         if (!IsGrounded()) slamCoroutine ??= StartCoroutine(Slam());
//     }
// 
// 
//     private IEnumerator Jump()
//     {
//         rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
//         rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
// 
//         yield return new WaitForSeconds(jumpCooldown);
//         jumpCoroutine = null;
//     }
// 
//     private IEnumerator Dash()
//     {
//         hasDash = false;
// 
//         Vector2 moveInput = input.Player.Movement.ReadValue<Vector2>();
//         Vector3 forward = rb.transform.forward;
//         Vector3 right = rb.transform.right;
// 
//         forward.y = 0;
//         right.y = 0;
// 
//         Vector3 dashDir = (forward * moveInput.y + right * moveInput.x).normalized;
// 
//         if (dashDir == Vector3.zero) dashDir = rb.transform.forward;
// 
//         rb.linearVelocity = Vector3.zero;
//         rb.AddForce(dashDir * dashForce, ForceMode.Impulse);
// 
//         StartCoroutine(DashEffect());
// 
//         yield return new WaitForSeconds(dashCooldown);
//         dashCoroutine = null;
//     }
// 
//     private IEnumerator Slam()
//     {
//         while (!IsGrounded())
//         {
//             if (input.Player.Dash.ReadValue<float>() > 0f)
//             {
//                 slamCoroutine = null;
//                 yield break;
//             }
// 
//             rb.linearVelocity = Vector3.down * Time.fixedDeltaTime * slamFallSpeed;
//             yield return null;
//         }
// 
//         slamCoroutine = null;
//     }
// 
//     private IEnumerator DashEffect()
//     {
//         // This fucking sucks actually never cook again
// 
//         float initialFOV = cam.fieldOfView;
//         float targetFOV = initialFOV + dashFOVIncrease;
//         float duration = 0.2f;
// 
//         for (float t = 0; t < duration; t += Time.deltaTime)
//         {
//             cam.fieldOfView = Mathf.Lerp(initialFOV, targetFOV, t / duration);
//             motionBlur.intensity.value = Mathf.Lerp(0f, 1f, t / duration);
//             chromAberration.intensity.value = Mathf.Lerp(0f, 1f, t / duration);
//             vignette.intensity.value = Mathf.Lerp(0f, 0.15f, t / duration);
//             yield return null;
//         }
// 
//         cam.fieldOfView = targetFOV;
//         motionBlur.intensity.value = 1f;
//         chromAberration.intensity.value = 1f;
// 
//         for (float t = 0; t < duration; t += Time.deltaTime)
//         {
//             cam.fieldOfView = Mathf.Lerp(targetFOV, initialFOV, t / duration);
//             motionBlur.intensity.value = Mathf.Lerp(1f, 0f, t / duration);
//             chromAberration.intensity.value = Mathf.Lerp(1f, 0f, t / duration);
//             vignette.intensity.value = Mathf.Lerp(0.15f, 0f, t / duration);
//             yield return null;
//         }
// 
//         cam.fieldOfView = initialFOV;
//         motionBlur.intensity.value = 0f;
//         chromAberration.intensity.value = 0f;
//         vignette.intensity.value = 0f;
//     }
// 
//     private void LockCursor()
//     {
//         Cursor.visible = false;
//         Cursor.lockState = CursorLockMode.Locked;
//     }
// 
//     private void EnableKeybinds()
//     {
//         input.Player.Enable();
// 
//         input.Player.Jump.performed += OnJumpPerformed;
//         input.Player.Dash.performed += OnDashPerformed;
//         input.Player.Slam.performed += OnSlamPerformed;
//     }
// 
//     private bool IsGrounded()
//     {
//         return Physics.CheckSphere(groundCheckSphere.position, 0.2f);
//     }
// 
//     private void OnDisable()
//     {
//         input.Player.Jump.performed -= OnJumpPerformed;
//         input.Player.Dash.performed -= OnDashPerformed;
//         input.Player.Slam.performed -= OnSlamPerformed;
// 
//         input.Player.Disable();
//     }
// }
// 
// public static class PlayerReferences
// {
//     public static Transform playerHandAnchor;
//     public static GameObject playerReference;
//     public static GameObject playerPhysical;
//     public static Camera playerCamera;
//     public static CardStructure playerHand;
//     public static CardStructure playerDeck;
// }
