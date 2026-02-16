using System.Collections.Generic;
using UnityEngine;

public class MonoCaller : MonoBehaviour
{
    public List<IMonoSubscriber> list;

    private void Awake()
    {
        foreach (var i in list) i.ManagedAwake();
    }

    private void OnEnable()
    {
        foreach (var i in list) i.ManagedOnEnable();
    }

    private void OnDisable()
    {
        foreach (var i in list) i.ManagedOnDisable();
    }
}

public interface IMonoSubscriber
{
    public void ManagedAwake() {}
    public void ManagedOnEnable() {}
    public void ManagedOnDisable() {}
}