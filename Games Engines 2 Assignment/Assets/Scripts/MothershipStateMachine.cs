using UnityEngine;
using System.Collections;

public class MothershipStateMachine : MonoBehaviour {

    public int health = 200;
    public GameObject enemy;
    public bool isStationery = true;

    [Header("Shooting options")]
    public float minAttackDelay = 2;
    public float maxAttackDelay = 5;
    public float bulletSpeed = 1000;
    public GameObject bullet;

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

    IEnumerator Attack() {
        while (enemy != null) {

            // Shot in enemy direction with a precision factor

            yield return new WaitForSeconds(Random.Range(minAttackDelay,maxAttackDelay));
        }
    }

}
