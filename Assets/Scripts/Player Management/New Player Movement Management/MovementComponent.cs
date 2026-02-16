using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public abstract class MovementComponent : MonoBehaviour
{
    public Player player;

    [Header("Easy access")]
    protected Rigidbody rb;
    protected Camera cam;
    protected MovementContext mctx;
    protected PlayerInputActions.MovementActions input;

    [Header("Reactive")]
    public InputAction triggerKey;
    public Action<InputAction.CallbackContext> listener;
    public Action callback;
    public Timer cooldown;

    private void Awake()
    {
        player = GetComponent<Player>();
        rb = player.physical.rb;
        cam = player.physical.cam;
        mctx = player.movement.ctx;
        input = player.movement.ctx.input.Movement;

        DelayedAwake();
    }

    protected virtual void DelayedAwake() {}

    protected void SetCallback(Action callback)
    {
        this.callback = callback;
        listener = _ => callback?.Invoke();
    }

    protected void SetKey(InputAction key) => triggerKey = key;

    protected void AssignKey(InputAction key, Action callback)
    {
        SetKey(key);
        SetCallback(callback);
    }

    // Managed Enable/Disable
    protected virtual void OnEnable()
    {
        if (triggerKey != null && listener != null) triggerKey.performed += listener;
    }

    protected virtual void OnDisable() 
    {
        if (triggerKey != null && listener != null) triggerKey.performed -= listener;
    }
}