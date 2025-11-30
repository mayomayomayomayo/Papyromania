using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PathManager
{
    public static List<GameObject> rooms;
    public static List<GameObject> path;

    public static float currentPathOrientation;

    public static void LoadAllSegments()
    {
        Random.InitState(12345);


        rooms = new();
        path = new(); // TODO move this somewhere else.

        foreach (Object obj in Resources.LoadAll("Prefabs/Rooms")) rooms.Add((GameObject)obj);

        // DebugMakePath();
    }

    public static void DebugMakePath()
    {
        for (int i = 0; i < 10; i++)
        {
            List<GameObject> validRooms = rooms.Where(r => Mathf.Abs(Mathf.DeltaAngle(0, currentPathOrientation + r.GetComponent<PathSegment>().turn)) < 179f).ToList();
            if (validRooms.Count == 0) throw new System.Exception("No available rooms match criteria");

            AppendToPath(validRooms[Random.Range(0, validRooms.Count)]);
        }
    }

    public static void AppendToPath(GameObject room)
    {
        Transform lastSegmentExitNode = path.Count > 0 ? path[^1].GetComponent<PathSegment>().exitNode.transform : new GameObject("Path").transform;

        GameObject newRoom = Object.Instantiate(
            room,
            lastSegmentExitNode.position,
            lastSegmentExitNode.rotation
        );

        newRoom.name = $"Room{path.Count:D3}";

        path.Add(newRoom);

        currentPathOrientation = (currentPathOrientation + newRoom.GetComponent<PathSegment>().turn) % 360f;
    }
}
