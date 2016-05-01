using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Boid))]
public class FighterStateMachine : MonoBehaviour {

    public int health = 10;
    public bool isCaptain = false;
    public float patrolRadius = 800.0f;
    public float collisionDistance = 100.0f;
    public GameObject captainShip = null;
    public Vector3 formationSpot;
    public GameObject motherShip = null;
    public GameObject currentEnemy = null;
    public GameObject bulletPrefab;
    

    List<Transform> turrets;

    [HideInInspector]
    public bool ready = false, isFighting = false, attackingTarget = false;
    State state = null;

    // Use this for initialization
    void Start() {
        turrets = new List<Transform>();
        foreach (Transform child in transform.FindChild("Turrets")) {
            turrets.Add(child);
        }
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

    public void ResetFighter() {
        isFighting = false;
        attackingTarget = false;
        currentEnemy = null;
        if (isCaptain) {
            SwitchState(new PatrollingState(this));
        }
        else {
            SwitchState(new FormationFollowState(this));
        }
    }

    public void EndBattle() {
        FighterStateMachine enemyMachine = currentEnemy.GetComponent<FighterStateMachine>();
        ResetFighter();
        enemyMachine.ResetFighter();
    }

    IEnumerator Fight() {
        // Check every 5 sec if the ship is destroyed
        while (health > 0) {
            // Check if the ship is attacking
            if (attackingTarget && currentEnemy != null) {
                Transform turret = turrets[Random.Range(0, turrets.Count)];
                turret.LookAt(currentEnemy.transform.position);
                GameObject bulletCopy = (GameObject)Instantiate(bulletPrefab, turret.position, turret.rotation);
                bulletCopy.GetComponent<LaserMover>().targetTag = currentEnemy.tag;
            }
            // Check the status of the current battle
            if (isFighting && currentEnemy != null && gameObject.tag != "Viper") {
                float distance = Vector3.Distance(transform.position, currentEnemy.transform.position);
                if (distance > 150.0f) {
                    EndBattle();
                }
            }
            yield return new WaitForSeconds(3);
        }
        // Respawn again 
        gameObject.SetActive(false);
        if (motherShip != null) {
            motherShip.GetComponent<MothershipSpawner>().Respawn(gameObject);
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
        StartCoroutine("Fight");
    }

    void OnDrawGizmos() {
        if (currentEnemy != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 20);
        }
        else {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, 20);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (!isFighting) {
            if (currentEnemy == null && other.GetType() == typeof(SphereCollider)) {
                if (other.gameObject.tag == "Viper" && other.gameObject.tag != gameObject.tag) {
                    if (!other.gameObject.GetComponent<FighterStateMachine>().isFighting) {
                        PickBattle(other.gameObject);
                    }
                }
            }
        }
    }

    void PickBattle(GameObject enemyFighter) {
        FighterStateMachine enemyMachine = enemyFighter.GetComponent<FighterStateMachine>();

        float random = Random.value;
        if (random > 0.5f) {
            FightEnemy(enemyFighter, new FightingState(this));
            enemyMachine.FightEnemy(gameObject, new PatrollingState(enemyMachine));
        }
        else {
            FightEnemy(enemyFighter, new PatrollingState(this));
            enemyMachine.FightEnemy(gameObject, new FightingState(enemyMachine));
        }
    }

    public void FightEnemy(GameObject enemy, State state) {
        currentEnemy = enemy;
        isFighting = true;
        SwitchState(state);
    }


}
