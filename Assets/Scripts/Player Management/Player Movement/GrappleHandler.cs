using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; 

// [RequireComponent(typeof(Player))]
// public class GrappleHandler : MonoBehaviour
// {
//     [Header("Grapple")]
//     public GrappleHook hook;
//     public GrappleState state;
// 
//     [Header("Events")]
//     public Action onBeginLaunch;
//     public Action<float> onLaunch;
//     public Action onPull;
//     public Action onCancelThrow;
//     public Action onCancelPull;
// 
//     [Header("References")]
//     public Player player;
// 
//     private void OnEnable()
//    {
//        SubscribeActions();
//    }
// 
//     private void OnDisable()
//    {
//        UnsubscribeActions();
//    }
// 
//     private void ProcessInput(InputAction.CallbackContext ctx)
//     {
//         Action toInvoke = state switch
//         {
//             GrappleState.Stored => onBeginLaunch,
//             GrappleState.Latched => onPull,
//             GrappleState.Thrown => onCancelThrow,
//             GrappleState.LatchedPulling => onCancelPull,
//             _ => null
//         };
// 
//         toInvoke?.Invoke();
//     }
// 
//     private void ThrowGrapple() => StartCoroutine(ThrowGrapple_cr());
// 
//     private IEnumerator ThrowGrapple_cr()
//    {
//        float wind = 0f;
//
//        yield return WindGrapple(result => wind = result);
//
//        if (wind <= hook.minWind) yield break;
//
//        hook.gameObject.SetActive(true);
//
//        onLaunch?.Invoke(wind);
//    }
// 
//     private IEnumerator WindGrapple(Action<float> onRelease)
//    {
//        float wind = 0f;
//
//        while (player.input.Movement.Grapple.IsPressed())
//        {
//            wind += hook.windIncreasePerTick;
//            yield return new WaitForFixedUpdate();
//        }
//
//        onRelease?.Invoke(Mathf.Clamp(wind, 0f, hook.maxWind));
//    }
// 
//     private void SubscribeActions()
//     {
//         player.input.Movement.Grapple.performed += ProcessInput;
//         onBeginLaunch += ThrowGrapple;
//     }
// 
//     private void UnsubscribeActions()
//     {
//         player.input.Movement.Grapple.performed -= ProcessInput;
//         onBeginLaunch -= ThrowGrapple;
//     }
// 
//     public enum GrappleState
//     {
//         Stored,
//         Thrown,
//         Recovering,
//         Latched,
//         LatchedPulling,
//     }
// }