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

    public static Vector2 ForceMagnitudeOf1(this Vector2 v) => new(v.x != 0 ? Mathf.Sign(v.x) : 0f, v.y != 0 ? Mathf.Sign(v.y) : 0f);
    public static Vector3 ForceMagnitudeOF1(this Vector3 v) => new(v.x != 0 ? Mathf.Sign(v.x) : 0f, v.y != 0 ? Mathf.Sign(v.y) : 0f, v.z != 0 ? Mathf.Sign(v.z) : 0f);
}

public static class ColliderUtils
{
    public static void ExtractCapsuleInformation(this CapsuleCollider c, out Vector3 point1, out Vector3 point2, out float radius)
    {
        radius = c.radius * Mathf.Max(
            c.transform.lossyScale.x,
            c.transform.lossyScale.z
        );

        float height = Mathf.Max(c.height * c.transform.lossyScale.y, radius * 2f);

        Vector3 center = c.transform.TransformPoint(c.center);
        float halfHeight = height / 2f - radius;

        point1 = center + c.transform.up * halfHeight;
        point2 = center - c.transform.up * halfHeight;
    }
    
}