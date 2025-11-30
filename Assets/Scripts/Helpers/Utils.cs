using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public static class TransformUtils // Useless rn btw.
{
    public static IEnumerator Move(Transform obj, Transform target, float moveSpeed = 10f, float snapDistance = 0.01f)
    {
        while (Vector3.Distance(obj.position, target.position) >= snapDistance)
        {
            obj.position = Vector3.Lerp(obj.position, target.position, moveSpeed);
            yield return null;
        }

        obj.position = target.position;
    }
}

public static class CoroutineUtils
{
    public static void KillCoroutine(this MonoBehaviour caller, ref Coroutine cr)
    {
        if (cr == null) return;

        caller.StopCoroutine(cr);
        cr = null;
    }
}