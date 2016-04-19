using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Boid))]
public class FSM : MonoBehaviour {

    public int health = 10;
    public bool isCaptain = false;
    public float patrolRadius = 1000.0f;
    public float collisionDistance = 100.0f;
    public GameObject captainShip = null;
    public Vector3 formationSpot;
    public Transform motherShip = null;

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

    IEnumerator StartUp() {

        // Initially wait a second in case other objects want to change this one
        yield return new WaitForSeconds(1);

        SwitchState(new StartupState(this));

        while (!ready) {
            // Wait few seconds until the ship has lift of
            yield return new WaitForSeconds(5);
        }
        StartCoroutine("AvoidMothershipCollision");
        if (isCaptain) {
            SwitchState(new PatrollingState(this));
        }
        else {
            SwitchState(new FormationFollowState(this));
        }
    }
    
    IEnumerator AvoidMothershipCollision() {

        while (true) {
            Ray forwardRay = new Ray(transform.position, transform.forward.normalized);
            RaycastHit hit;
            if (Physics.Raycast(forwardRay, out hit, collisionDistance)) {
                if (hit.collider.tag == "Mothership") {
                    GetComponent<Boid>().collisionPriority = true;
                    GetComponent<Boid>().seekTargetPos = hit.point + (hit.normal * 20);
                    GetComponent<Boid>().seekEnabled = true;
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

}
