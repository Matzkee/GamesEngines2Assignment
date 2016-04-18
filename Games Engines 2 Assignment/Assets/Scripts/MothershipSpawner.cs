using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MothershipSpawner : MonoBehaviour {

    public GameObject spawnPrefab;

    [Range(0,6)]
    public int teams;
    [Range(0, 10)]
    public int teamSize;

    Stack<GameObject> toSpawn;

	// Use this for initialization
	void Start () {
        toSpawn = new Stack<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

