using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Boid))]
public class FighterStateMachine : MonoBehaviour {

    public int health = 10;
    public bool isCaptain = false;
    public float patrolRadius = 1000.0f;
    public float collisionDistance = 100.0f;
    public GameObject captainShip = null;
    public Vector3 formationSpot;
    public GameObject motherShip = null;
    public string enemyType;
    public GameObject currentEnemy = null;

    [HideInInspector]
    public bool ready = false;
    State state = null;

    // Use this for initialization
    void Start() {
        StartCoroutine("StartUp");
    }
    void OnEnable() {
        StartCoroutine("StartUp");
    }

    public void SwitchState(State state) {
        if (this.state != null) {
            this.state.Exit();
        }

        this.state = state;

        if (this.state != null) {
            this.state.Enter();
        }
    }

    // Update is called once per frame
    void Update() {
        if (state != null) {
            state.Update();
        }
    }

    IEnumerator Fight() {
        while (health > 0) {
            // Check every 5 sec if the ship is destroyed
            yield return new WaitForSeconds(5);
        }
        // Respawn again 
        gameObject.SetActive(false);
        motherShip.GetComponent<MothershipSpawner>().Respawn(gameObject);
    }

    IEnumerator StartUp() {

        // Initially wait a second in case other objects want to change this one
        yield return new WaitForSeconds(1);

        SwitchState(new StartupState(this));

        while (!ready) {
            // Wait few seconds until the ship has lift of
            yield return new WaitForSeconds(5);
        }
        StartCoroutine("AvoidMothershipCollision");
        StartCoroutine("Fight");
    }
    
    IEnumerator AvoidMothershipCollision() {

        while (true) {
            Ray forwardRay = new Ray(transform.position, transform.forward.normalized);
            RaycastHit hit;
            if (Physics.Raycast(forwardRay, out hit, collisionDistance)) {
                if (hit.collider.tag == "Mothership") {
                    Vector3 toHitpoint = (hit.point - transform.position).normalized;
                    Vector3 reflectedDir = Vector3.Reflect(toHitpoint, hit.normal);
                    Vector3 reflectedPos = hit.point + (reflectedDir * 50);
                    GetComponent<Boid>().collisionPriority = true;
                    GetComponent<Boid>().seekTargetPos = reflectedPos;
                    GetComponent<Boid>().seekEnabled = true;
                    Debug.DrawLine(transform.position, hit.point, Color.green, 2);
                    Debug.DrawLine(hit.point, reflectedPos, Color.green, 2);
                }
                else {
                    GetComponent<Boid>().seekEnabled = false;
                    GetComponent<Boid>().collisionPriority = false;
                }
            }
            else {
                GetComponent<Boid>().seekEnabled = false;
                GetComponent<Boid>().collisionPriority = false;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    void OnDrawGizmos() {
        if (isCaptain) {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 40);
        }
        else {
            Gizmos.color = new Color(0, 1, 1, 0.5f);
            Gizmos.DrawWireSphere(transform.position, 10);
        }
    }

    void OnTriggerEnter(Collider other) {
        // Sphere collider is the detection collider
        if (other.GetType() == typeof(SphereCollider) && other.gameObject.tag == enemyType) {
            if (currentEnemy == null) {
                currentEnemy = other.gameObject;
            }
        }
    }

}
