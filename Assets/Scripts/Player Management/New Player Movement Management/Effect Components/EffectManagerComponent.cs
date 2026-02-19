using UnityEngine;

// [ShutUpUnity(AttributesDontFuckingInherit)]
public abstract class EffectManagerComponent<T> : DynamicActionSubHandler where T : MonoBehaviour
{
    protected T target;

    [SerializeField]
    protected Camera cam;

    private void Awake()
    {
        target = GetComponent<T>();
        cam = GetComponent<Player>().physical.cam;
    }
}