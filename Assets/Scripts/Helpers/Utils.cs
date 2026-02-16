using UnityEngine;
using System.Collections;

public static class TransformUtils
{
    public static IEnumerator MoveTo(Transform obj, Transform target, float moveSpeed = 10f, float snapDistance = 0.01f)
    {
        while (Vector3.Distance(obj.position, target.position) >= snapDistance)
        {
            obj.position = Vector3.Lerp(obj.position, target.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        obj.position = target.position;
    }

    public static IEnumerator MoveTo(Transform obj, Vector3 target, float moveSpeed = 10f, float snapDistance = 0.01f)
    {
        while (Vector3.Distance(obj.position, target) >= snapDistance)
        {
            obj.position = Vector3.Lerp(obj.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        obj.position = target;
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

public static class VectorUtils
{
    public static Vector3 NeuterX(this Vector3 v) => new (0f, v.y, v.z);
    public static Vector3 NeuterY(this Vector3 v) => new (v.x, 0f, v.z);
    public static Vector3 NeuterZ(this Vector3 v) => new (v.x, v.y, 0f);
}