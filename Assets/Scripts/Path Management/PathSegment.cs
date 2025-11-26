using System;
using UnityEngine;

[Serializable]
public class PathSegment : MonoBehaviour
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
    
    [Header("Meta")]
    public float turn;

    public void Awake()
    {
        entryNode = transform.Find("EntryNode");
        exitNode = transform.Find("ExitNode");
        turn = Mathf.DeltaAngle(entryNode.localEulerAngles.y, exitNode.localEulerAngles.y);

        Debug.Log($"{gameObject.name} has turn = {turn}");
    }
}
