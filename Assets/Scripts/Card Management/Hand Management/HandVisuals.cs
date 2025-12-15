using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class HandVisuals
{
    public Hand hand;

    public float cardOrderSpeed;

    [Header("LAYOUTS")]
    public HandLayoutData restingLayout;
    public HandLayoutData aimingLayout;

    public readonly List<Coroutine> coroutines = new();
}

[Serializable]
public struct HandLayoutData
{
    public TransformData rawOffset;
    public AxisCurves positionCurves;
    public AxisCurves rotationCurves;
    public AxisCurves scaleCurves;
}

[Serializable]
public struct AxisCurves
{
    public AnimationCurve x;
    public AnimationCurve y;
    public AnimationCurve z;

    public AxisCurves(AnimationCurve x, AnimationCurve y, AnimationCurve z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}