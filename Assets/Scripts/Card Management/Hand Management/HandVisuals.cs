using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class HandVisuals
{
    public Hand hand;

    [Header("Visual data")]
    public float handSpreadResting;
    public float handSpreadAiming;
    public float handRotation;
    public float handDrop;
    public float cardOrderSpeed;

    public readonly List<Coroutine> coroutines = new();
}