using UnityEngine;
using System.Collections;

public class LaserMover : MonoBehaviour {

    public float lifeTime = 3.0f;
    public int damage = 2;
    public float speed = 1000.0f;
    public string targetTag;

    public GameObject target = null;
    public GameObject explosion;
    public GameObject explosion2;

    // Update is called once per frame
    void Update () {
        Vector3 movement = transform.position + (transform.forward * Time.deltaTime * speed);
        transform.position = movement;

        if (target != null) {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist < 10.0f) {
                target.GetComponent<FighterStateMachine>().health -= damage;
                Instantiate(explosion2, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
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
            // destroy object
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

}
