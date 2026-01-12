using UnityEngine;
using System.Collections;

public class GrappleHook : MonoBehaviour
{
    [Header("Wind-up")]
    public float minWind;
    public float maxWind;
    public float windIncreasePerTick;

    [Header("Stats")]
    public float baseLaunchForce;
    public float pullSpeed;
    public float pullbackSpeed;
    public float maxRange;
    public AnimationCurve windStrengthCurve;

    [Header("References")]
    public Player player;
    public Rigidbody rb;
    public Transform parent;
    public SphereCollider recoveryCollider;
    public GrappleHandler handler;

    protected Coroutine activeRoutine;
    protected Vector3 pullingPositionDelta;

    private void Awake() => PairToHandler(handler); // FOR NOW

    private void OnCollisionEnter(Collision other)
    {
        LatchHook(other.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) ResetHook();
    }

    protected virtual void LaunchHook(float wind)
    {
        handler.state = GrappleHandler.GrappleState.Thrown;

        float strength = windStrengthCurve.Evaluate(wind) * baseLaunchForce;

        transform.parent = null;
        rb.isKinematic = false;

        rb.AddForce(player.playerCamera.transform.forward * strength, ForceMode.VelocityChange);
    }

    protected virtual void BeginWind()
    {
        // Unused for default hook, but surely a good thing to have by default
    }

    protected virtual void PullPlayer() => StartCoroutine(PullPlayer_cr());

    protected virtual IEnumerator PullPlayer_cr()
    {
        handler.state = GrappleHandler.GrappleState.LatchedPulling;
        
        player.playerRigidbody.isKinematic = true;

        while (handler.state == GrappleHandler.GrappleState.LatchedPulling)
        {
            Vector3 initPos = player.transform.position;

            Vector3 pullDirection = (transform.position - player.transform.position).normalized;
            player.playerRigidbody.MovePosition(player.transform.position + pullDirection * pullSpeed * Time.fixedDeltaTime);

            Vector3 finalPos = player.transform.position;

            pullingPositionDelta = finalPos - initPos;

            yield return new WaitForFixedUpdate();
        }

        player.playerRigidbody.isKinematic = false;
    }

    protected virtual void CancelPull()
    {
        handler.state = GrappleHandler.GrappleState.Stored;
        
        player.playerRigidbody.isKinematic = false;
        player.playerRigidbody.linearVelocity = pullingPositionDelta;

        ResetHook();
    }

    protected virtual void LatchHook(Transform t)
    {
        handler.state = GrappleHandler.GrappleState.Latched;
        rb.isKinematic = true;
        recoveryCollider.enabled = true;
    }

    protected virtual void ResetHook()
    {
        recoveryCollider.enabled = false;

        transform.parent = parent;
        transform.localPosition = Vector3.zero;

        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;

        handler.state = GrappleHandler.GrappleState.Stored;

        gameObject.SetActive(false);
    }

    protected virtual void PairToHandler(GrappleHandler gh)
    {
        gh.onLaunch += LaunchHook;
        gh.onBeginLaunch += BeginWind;
        gh.onPull += PullPlayer;
        gh.onCancelPull += CancelPull;
        gh.onCancelThrow += ResetHook;
    }
}