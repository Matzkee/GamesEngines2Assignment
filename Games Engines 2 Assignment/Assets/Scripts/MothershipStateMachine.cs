using UnityEngine;
using System.Collections;

public class MothershipStateMachine : MonoBehaviour {

    public int health = 200;
    public GameObject enemy;
    public bool isStationery = true;

    [HideInInspector]
    State state = null;

    void Start() {
        // Pick a point at a certain distance if the ship is approaching
        if (!isStationery) {
            Vector3 toTarget = (enemy.transform.position - transform.position).normalized;
            Vector3 approachPoint = enemy.transform.position - (toTarget * 1000);
            GetComponent<Boid>().arriveTargetPos = approachPoint;
            GetComponent<Boid>().arriveEnabled = true;
        }
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
}
