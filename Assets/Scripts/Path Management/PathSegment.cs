using System;
using UnityEngine;

[Serializable]
public class PathSegment : ScriptableObject
{
    public enum SegmentType
    {
        Default,
        Combat,
        Parkour,
        TowerBottom,
        TowerClimb,
        TowerPeak
    }

    [Header("Nodes")]
    public Transform entryNode;
    public Transform exitNode;

    [Header("Type")]
    public SegmentType type;
    
}
