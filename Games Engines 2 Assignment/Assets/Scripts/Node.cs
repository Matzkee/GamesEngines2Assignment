using UnityEngine;
using System.Collections;

public class Node {

    public Node parent;
    public Vector3 pos;
    public int gCost;
    public int hCost;
    public int fCost {
        get {
            return gCost + fCost;
        }
    }

    public Node() {
    }
}
