using UnityEngine;
using System.Collections;

public class ObjectDestroyer : MonoBehaviour {

    public float delay = 5;

	// Use this for initialization
	void Awake() {
        Destroy(gameObject, delay);
    }
}
