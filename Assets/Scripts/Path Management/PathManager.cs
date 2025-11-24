using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class PathManager
{
    public static List<PathSegment> rooms;
    public static List<PathSegment> path;

    public static float currentPathOrientation;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void LoadAllSegments()
    {
        rooms = new();

        foreach (Object obj in Resources.LoadAll("Prefabs/Rooms")) rooms.Add(obj.GetComponent<PathSegment>());
        foreach (PathSegment ps in rooms) ps.Init();
    }

    public static void DebugMakePath()
    {
        
    }

    public static void AppendToPath()
    {
        currentPathOrientation = path[^1].exitNode.transform.localEulerAngles.z;

        
        path.Add(rooms[Random.Range(0, rooms.Count - 1)]);
    }
}
