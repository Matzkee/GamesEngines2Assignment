using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinder {

    public int voxelSize;
    Dictionary<Vector3, Node> openSet;
    Dictionary<Vector3, Node> closedSet;
    Vector3 start, end;
    List<Collider> obstacles;

    public Pathfinder(int voxelSize) {
        this.voxelSize = voxelSize;
        openSet = new Dictionary<Vector3, Node>(20000);
        closedSet = new Dictionary<Vector3, Node>(20000);
    }

    Vector3 PositionToVoxel(Vector3 v) {
        Vector3 ret = new Vector3();
        ret.x = ((int)(v.x / voxelSize)) * voxelSize;
        ret.y = ((int)(v.y / voxelSize)) * voxelSize;
        ret.z = ((int)(v.z / voxelSize)) * voxelSize;
        return ret;
    }

    public Path FindPath(Vector3 start, Vector3 end, List<Collider> obstacles) {
        this.obstacles = obstacles;
        bool found = false;
        this.end = PositionToVoxel(start); // end refers to start
        this.start = PositionToVoxel(end); // start refers to end
        // Clear dictionaries
        openSet.Clear();
        closedSet.Clear();

        Node first = new Node();
        first.gCost = first.hCost = 0.0f;
        first.pos = this.start;
        openSet[this.start] = first;

        Node current = first;
        while (openSet.Count > 0) {
            float min = float.MaxValue;
            foreach (Node node in openSet.Values) {
                if (node.fCost < min) {
                    current = node;
                    min = node.fCost;
                }
            }

            if (current.pos.Equals(this.end)) {
                found = true;
                break;
            }
            AddAdjacentNodes(current);
            openSet.Remove(current.pos);
            closedSet[current.pos] = current;
        }

        Path path = new Path();
        if (found) {
            while (!current.pos.Equals(this.start)) {
                path.waypoints.Add(current.pos);
                current = current.parent;
            }
            path.waypoints.Add(current.pos);
        }
        else {
            Debug.Log("No path found");
        }
        return path;
    }

    void AddAdjacentNodes(Node current) {
        // Forwards
        Vector3 pos;
        pos.x = current.pos.x;
        pos.y = current.pos.y;
        pos.z = current.pos.z + voxelSize;
        AddIfValid(pos, current);

        // Forwards right
        pos.x = current.pos.x + voxelSize;
        pos.y = current.pos.y;
        pos.z = current.pos.z + voxelSize;
        AddIfValid(pos, current);

        // Right
        pos.x = current.pos.x + voxelSize;
        pos.y = current.pos.y;
        pos.z = current.pos.z;
        AddIfValid(pos, current);

        // Backwards Right
        pos.x = current.pos.x + voxelSize;
        pos.y = current.pos.y;
        pos.z = current.pos.z - voxelSize;
        AddIfValid(pos, current);

        // Backwards
        pos.x = current.pos.x;
        pos.y = current.pos.y;
        pos.z = current.pos.z - voxelSize;
        AddIfValid(pos, current);

        // Backwards Left
        pos.x = current.pos.x - voxelSize;
        pos.y = current.pos.y;
        pos.z = current.pos.z - voxelSize;
        AddIfValid(pos, current);

        // Left
        pos.x = current.pos.x - voxelSize;
        pos.y = current.pos.y;
        pos.z = current.pos.z;
        AddIfValid(pos, current);

        // Forwards Left
        pos.x = current.pos.x - voxelSize;
        pos.y = current.pos.y;
        pos.z = current.pos.z + voxelSize;
        AddIfValid(pos, current);
        // Above in front row
        pos.x = current.pos.x - voxelSize;
        pos.y = current.pos.y + voxelSize;
        pos.z = current.pos.z - voxelSize;
        AddIfValid(pos, current);

        pos.x = current.pos.x;
        pos.y = current.pos.y + voxelSize;
        pos.z = current.pos.z - voxelSize;
        AddIfValid(pos, current);

        pos.x = current.pos.x + voxelSize;
        pos.y = current.pos.y + voxelSize;
        pos.z = current.pos.z - voxelSize;
        AddIfValid(pos, current);


        // Above middle row
        pos.x = current.pos.x - voxelSize;
        pos.y = current.pos.y + voxelSize;
        pos.z = current.pos.z;
        AddIfValid(pos, current);

        pos.x = current.pos.x;
        pos.y = current.pos.y + voxelSize;
        pos.z = current.pos.z;
        AddIfValid(pos, current);

        pos.x = current.pos.x + voxelSize;
        pos.y = current.pos.y + voxelSize;
        pos.z = current.pos.z;
        AddIfValid(pos, current);

        // Above back row
        pos.x = current.pos.x - voxelSize;
        pos.y = current.pos.y + voxelSize;
        pos.z = current.pos.z + voxelSize;
        AddIfValid(pos, current);

        pos.x = current.pos.x;
        pos.y = current.pos.y + voxelSize;
        pos.z = current.pos.z + voxelSize;
        AddIfValid(pos, current);

        pos.x = current.pos.x + voxelSize;
        pos.y = current.pos.y + voxelSize;
        pos.z = current.pos.z + voxelSize;
        AddIfValid(pos, current);

        // Below in front row
        pos.x = current.pos.x - voxelSize;
        pos.y = current.pos.y - voxelSize;
        pos.z = current.pos.z - voxelSize;
        AddIfValid(pos, current);

        pos.x = current.pos.x;
        pos.y = current.pos.y - voxelSize;
        pos.z = current.pos.z - voxelSize;
        AddIfValid(pos, current);

        pos.x = current.pos.x + voxelSize;
        pos.y = current.pos.y - voxelSize;
        pos.z = current.pos.z - voxelSize;
        AddIfValid(pos, current);

        // Below middle row
        pos.x = current.pos.x - voxelSize;
        pos.y = current.pos.y - voxelSize;
        pos.z = current.pos.z;
        AddIfValid(pos, current);

        pos.x = current.pos.x;
        pos.y = current.pos.y - voxelSize;
        pos.z = current.pos.z;
        AddIfValid(pos, current);

        pos.x = current.pos.x + voxelSize;
        pos.y = current.pos.y - voxelSize;
        pos.z = current.pos.z;
        AddIfValid(pos, current);

        // Below back row
        pos.x = current.pos.x - voxelSize;
        pos.y = current.pos.y - voxelSize;
        pos.z = current.pos.z + voxelSize;
        AddIfValid(pos, current);

        pos.x = current.pos.x;
        pos.y = current.pos.y - voxelSize;
        pos.z = current.pos.z + voxelSize;
        AddIfValid(pos, current);

        pos.x = current.pos.x + voxelSize;
        pos.y = current.pos.y - voxelSize;
        pos.z = current.pos.z + voxelSize;
        AddIfValid(pos, current);
    }

    void AddIfValid(Vector3 pos, Node parent) {
        if (IsNavigable(pos)) {
            if (!closedSet.ContainsKey(pos)) {
                if (!openSet.ContainsKey(pos)) {
                    Node node = new Node();
                    node.pos = pos;
                    node.gCost = parent.gCost + cost(node.pos, parent.pos);
                    node.hCost = heuristic(pos, end);
                    node.parent = parent;
                    openSet[pos] = node;
                }
                else {
                    // Edge relaxation
                    Node node = openSet[pos];
                    float g = parent.gCost + cost(node.pos, parent.pos);
                    if (g < node.gCost) {
                        node.gCost = g;
                        node.parent = parent;
                    }
                }
            }
        }
    }

    bool IsNavigable(Vector3 pos) {
        // See if a vector is inside collider
        bool hit = false;
        foreach (Collider obstacle in obstacles) {
            if (obstacle.bounds.Contains(pos)) {
                hit = true;
                break;
            }
        }

        return !hit;
    }
    float heuristic(Vector3 v1, Vector3 v2) {
        return 10.0f * (Mathf.Abs(v2.x - v1.x) + Mathf.Abs(v2.y - v1.y) + Mathf.Abs(v2.z - v1.z));
    }
    float cost(Vector3 v1, Vector3 v2) {
        int dist = (int)Mathf.Abs(v2.x - v1.x) + (int)Mathf.Abs(v2.y - v1.y) + (int)Mathf.Abs(v2.z - v1.z);
        return (dist == 1) ? 10 : 14;
    }
}
