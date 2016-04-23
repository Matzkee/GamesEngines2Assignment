using UnityEngine;
using System.Collections;

public class LaserMover : MonoBehaviour {

    public float speed = 1000;
    public string targetTag;
	
	// Update is called once per frame
	void Update () {
        transform.Translate(transform.forward * Time.deltaTime * speed);
	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == targetTag) {
            // Explosion effect, reduce health, deactivate this component
        }
    }

}
