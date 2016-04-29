using UnityEngine;
using System.Collections;

public class LaserMover : MonoBehaviour {

    public float lifeTime = 3.0f;
    public int damage = 2;
    public float speed = 1000.0f;
    public string targetTag;

    public GameObject explosion;
	
	// Update is called once per frame
	void Update () {
        Vector3 movement = transform.position + (transform.forward * Time.deltaTime * speed);
        transform.position = movement;
	}

    void Awake() {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other) {
        if (other.transform.root.tag == targetTag) {
            // reduce health of target tag
            if (targetTag == "Basestar" || targetTag == "Pegasus") {
                other.transform.root.GetComponent<MothershipBrain>().health -= damage;
            }
            if (targetTag == "Viper" || targetTag == "Raider") {
                if (other.GetType() != typeof(SphereCollider)) {
                    other.transform.root.GetComponent<FighterStateMachine>().health -= damage;
                }
            }
            // destroy object
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

}
