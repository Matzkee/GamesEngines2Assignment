using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Boid))]
public class FSM : MonoBehaviour {

    public int health = 10;
    public bool isCaptain = false;
    public float patrolRadius = 1000.0f;
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
        SwitchState(new StartupState(this));

        while (!ready) {
            yield return new WaitForSeconds(5);
        }
        if (isCaptain) {
            SwitchState(new PatrollingState(this));
        }
        else {
            SwitchState(new FormationFollowState(this));
        }
    }
}
