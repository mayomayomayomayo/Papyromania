using UnityEngine;
using System;

public class CustomCardBehaviour : MonoBehaviour
{
    public virtual void OnUse() => throw new NotImplementedException("OnUse should be implemented by any derived class");
    public virtual void OnDiscard() => throw new NotImplementedException("OnDiscard currently has no default implementation");
}