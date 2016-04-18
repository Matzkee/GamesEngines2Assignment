using UnityEngine;
using System.Collections.Generic;

public class Tetst : MonoBehaviour {

    public int maxPoints;
    public float formationOffset;
    
    public List<Vector3> points;

	// Use this for initialization
	void Start () {
        points = new List<Vector3>();
        MakePyramidFormation(maxPoints, formationOffset);
	}

    void MakePyramidFormation(int _maxPoints, float offset)
    {
        int rowCount = 1;

        while (points.Count < _maxPoints)
        {
            Vector3 nextRow = new Vector3(-offset * rowCount, 0, -offset * rowCount);
            if (points.Count < _maxPoints)
                points.Add(nextRow);
            else
                break;
            Vector3 currentOffset = new Vector3(offset * 2, 0, 0);
            for (int i = 1; i <= rowCount; i++)
            {
                if (points.Count < _maxPoints)
                    points.Add(nextRow + (currentOffset * i));
                else
                    break;
            }

            rowCount++;
        }
    }

    void OnDrawGizmos()
    {
        if (points != null)
        {
            foreach (Vector3 point in points)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawWireSphere(point, 1);
            }
        }
    }
}
