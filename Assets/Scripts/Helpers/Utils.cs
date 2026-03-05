using UnityEngine;

public abstract class MonoVessel<T> : MonoBehaviour
{
    public T Item { get; private set; }

    public void Init(T item) => Item ??= item;

    public static implicit operator T(MonoVessel<T> mv) => mv.Item;
}

public static class VectorUtils
{
    public static Vector3 NeuterX(this Vector3 v) => new (0f, v.y, v.z);
    public static Vector3 NeuterY(this Vector3 v) => new (v.x, 0f, v.z);
    public static Vector3 NeuterZ(this Vector3 v) => new (v.x, v.y, 0f);

    public static Vector2 ForceMagnitudeOf1(this Vector2 v) => new(v.x != 0 ? Mathf.Sign(v.x) : 0f, v.y != 0 ? Mathf.Sign(v.y) : 0f);
    public static Vector3 ForceMagnitudeOf1(this Vector3 v) => new(v.x != 0 ? Mathf.Sign(v.x) : 0f, v.y != 0 ? Mathf.Sign(v.y) : 0f, v.z != 0 ? Mathf.Sign(v.z) : 0f);
}

public static class ColliderUtils
{
    public static CapsuleColliderData ExtractCapsuleInformation(this CapsuleCollider c)
    {
        float radius = c.radius * Mathf.Max(
            c.transform.lossyScale.x,
            c.transform.lossyScale.z
        );

        float height = Mathf.Max(c.height * c.transform.lossyScale.y, radius * 2f);

        Vector3 center = c.transform.TransformPoint(c.center);
        float halfHeight = height / 2f - radius;

        Vector3 point1 = center + c.transform.up * halfHeight;
        Vector3 point2 = center - c.transform.up * halfHeight;

        return new (point1, point2, radius);
    }
    
    public struct CapsuleColliderData
    {
        public Vector3 point1;
        public Vector3 point2;
        public float radius;

        public CapsuleColliderData(Vector3 p1, Vector3 p2, float r)
        {
            point1 = p1;
            point2 = p2;
            radius = r;
        }
    }
}