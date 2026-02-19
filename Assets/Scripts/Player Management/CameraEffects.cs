using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Welcome to generic land
// This was fun to make

public static class CameraEffects
{
    private static readonly Dictionary<Camera, Dictionary<Type, CameraEffect>> effectsByCamera = new();

    public static void QuickTriggerEffect<T>(this Camera cam) where T : CameraEffect, new()
    {
        CoroutineRunner.Instance.StartCoroutine(QuickTriggerCoroutine());

        IEnumerator QuickTriggerCoroutine()
        {
            CameraEffect effect = cam.AddEffect<T>();

            yield return new WaitUntil(() => effect.CurrentRoutine == null);

            cam.RemoveEffect<T>();
        }
    }

    public static T AddEffect<T>(this Camera cam) where T : CameraEffect, new()
    {
        if (!effectsByCamera.TryGetValue(cam, out var effects))
        {
            effects = effectsByCamera[cam] = new();
        }

        if (!effects.TryGetValue(typeof(T), out CameraEffect existing))
        {
            CameraEffect effect = new T();
            effects[typeof(T)] = effect;
            effect.OnAdd(cam);

            return (T)effect;
        }

        return (T)existing;
    }

    public static void RemoveEffect<T>(this Camera cam) where T : CameraEffect, new()
    {
        if (!effectsByCamera.TryGetValue(cam, out var effects))
        {
            effects = effectsByCamera[cam] = new();
        }

        if (effects.TryGetValue(typeof(T), out var effect))
        {
            effect.OnRemove(cam);
            effects.Remove(typeof(T));
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