using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(MovementContext))]
public abstract class MovementManager : MonoBehaviour
{
    public Player player;
    public Rigidbody rb;
    public Camera cam;

    public MovementContext moveCtx;
    public PlayerInputActions.MovementActions input;

    [Header("Reactive")]
    public InputAction key;
    public Action<InputAction.CallbackContext> listener;
    public Action callback;
    public Timer cooldown;

    private void Awake()
    {
        player = GetComponent<Player>();
        rb = player.playerRigidbody;
        cam = player.playerCamera;

        moveCtx = GetComponent<MovementContext>();
        input = player.inputContext.input.Movement;

        DelayedAwake();
    }

    protected virtual void DelayedAwake() {}

    protected virtual void SetCallback(Action callback) // Run before OnEnable
    {
        this.callback = callback;
        listener = _ => callback?.Invoke();
    }

    protected virtual void SetKey(InputAction key) => this.key = key;

    protected virtual void AssignKey(InputAction key, Action callback)
    {
        SetKey(key);
        SetCallback(callback);
    }

    protected virtual void OnEnable()
    {
        if (key != null && listener != null) key.performed += listener;
    }

    protected virtual void OnDisable() 
    {
        if (key != null && listener != null) key.performed -= listener;
    }
}

// public class InputBinding
// {
//     private readonly InputAction key;
//     private readonly Action<InputAction.CallbackContext> listener;
// 
//     public InputBinding(InputAction key, Action callback)
//     {
//         this.key = key;
//         listener = _ => callback();
//     }
// 
//     public void Enable() => key.performed += listener;
//     public void Disable() => key.performed -= listener;
// }