using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Welcome to generic land
// This was fun to make

public static class CameraEffects
{
    // This is a bit cursed isn't it...
    private static readonly Dictionary<Camera, Dictionary<string, CameraEffect>> effectsByCamera = new();

    public static void QuickTriggerEffect<T>(this Camera cam) where T : CameraEffect, new()
    {
        CameraEffect effect = new T();
        
        CoroutineRunner.Instance.StartCoroutine(QuickTriggerCoroutine());

        IEnumerator QuickTriggerCoroutine()
        {
            effect.OnAdd(cam);

            yield return new WaitUntil(() => effect.CurrentRoutine == null);

            effect.OnRemove(cam);
        }
    }

    public static void AddEffect<T>(this Camera cam) where T : CameraEffect, new()
    {
        if (!effectsByCamera.TryGetValue(cam, out var effects))
        {
            effects = effectsByCamera[cam] = new();
        }

        CameraEffect effect = new T();
        string effectName = effect.GetType().Name;

        if (!effects.TryGetValue(effectName, out _))

        effects[effectName] = effect;

        effect.OnAdd(cam);
    }

    public static void RemoveEffect<T>(this Camera cam) where T : CameraEffect, new()
    {
        if (!effectsByCamera.TryGetValue(cam, out var effects))
        {
            effects = effectsByCamera[cam] = new();
        }

        string effectName = new T().GetType().Name;

        if (effects.TryGetValue(effectName, out var effect))
        {
            effect.OnRemove(cam);
            effects.Remove(effectName);
        }
    }

    public abstract class CameraEffect
    {
        public Coroutine CurrentRoutine { get; protected set; }

        public void OnAdd(Camera cam)
        {
            if (CurrentRoutine != null) CoroutineRunner.Instance.StopCoroutine(CurrentRoutine);
            CurrentRoutine = CoroutineRunner.Instance.StartCoroutine(AddEffectWrapper(cam));
        }

        public void OnRemove(Camera cam)
        {
            if (CurrentRoutine != null) CoroutineRunner.Instance.StopCoroutine(CurrentRoutine);
            CurrentRoutine = CoroutineRunner.Instance.StartCoroutine(RemoveEffectWrapper(cam));
        }

        private IEnumerator AddEffectWrapper(Camera cam)
        {
            yield return AddEffect(cam);
            CurrentRoutine = null;
        }

        private IEnumerator RemoveEffectWrapper(Camera cam)
        {
            yield return RemoveEffect(cam);
            CurrentRoutine = null;
        }

        protected virtual IEnumerator AddEffect(Camera cam) { yield break; }
        protected virtual IEnumerator RemoveEffect(Camera cam) { yield break; }
    }
}