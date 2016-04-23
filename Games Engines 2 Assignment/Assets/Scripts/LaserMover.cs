using UnityEngine;
using System.Collections;

public class LaserMover : MonoBehaviour {

    public float lifeTime = 3.0f;
    public int damage = 2;
    public float speed = 1000.0f;
    public string targetTag;
	
	// Update is called once per frame
	void Update () {
        Vector3 movement = transform.position + (transform.forward * Time.deltaTime * speed);
        transform.position = movement;
	}

    void Awake() {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == targetTag) {
            // reduce health of target tag
            if (targetTag == "Basestar" || targetTag == "Pegasus") {
                other.gameObject.GetComponent<MothershipStateMachine>().health -= damage;
            }
            if (targetTag == "Viper" || targetTag == "Raider") {
                other.gameObject.GetComponent<FighterStateMachine>().health -= damage;
            }
            // destroy object
            Destroy(gameObject);
        }
    }

}
