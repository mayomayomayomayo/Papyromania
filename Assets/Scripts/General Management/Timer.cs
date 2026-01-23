using System;
using UnityEngine;

[Serializable]
public class Timer
{
    public float duration;
    private float start;
    private float end;

    public Timer(float duration) => this.duration = duration;

    public bool Ready => Time.time >= end;

    public float Progress => Mathf.Clamp01(Mathf.InverseLerp(start, end, Time.time));

    public void Start()
    {
        start = Time.time;
        end = start + duration;
    }
}