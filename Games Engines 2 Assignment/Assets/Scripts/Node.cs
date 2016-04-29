using UnityEngine;
using System.Collections;

public class Node {

    public Node parent;
    public Vector3 pos;
    public float gCost;
    public float hCost;
    public float fCost {
        get {
            return gCost + fCost;
        }
    }

    public Node() {
    }
}
