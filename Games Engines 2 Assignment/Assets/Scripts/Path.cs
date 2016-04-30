using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Path{
    public List<Vector3> waypoints = new List<Vector3>();
    public int next = 0;
    public bool reachedEnd = false;

    public Vector3 NextWaypoint() {
        return waypoints[next];
    }

    public bool IsLast {
        get {
            return (next == waypoints.Count - 1);
        }
    }

    public void AdvanceToNext() {
        if (next != waypoints.Count - 1) {
            next = next + 1;
        }
        else {
            reachedEnd = true;
        }
    }
}
