using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Boid))]
public class FighterStateMachine : MonoBehaviour {

    public int health = 10;
    public bool isCaptain = false;
    public float patrolRadius = 1000.0f;
    public float collisionDistance = 100.0f;
    public GameObject captainShip = null;
    public Vector3 formationSpot;
    public GameObject motherShip = null;
    public GameObject currentEnemy = null;
    public GameObject bulletPrefab;
    
    public Pathfinder pathfinder;
    int voxelSize = 150;

    List<Transform> turrets;

    [HideInInspector]
    public bool ready = false, isFighting = false, attackingTarget = false;
    State state = null;

    // Use this for initialization
    void Start() {
        pathfinder = new Pathfinder(voxelSize);
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
            if (attackingTarget) {
                Transform turret = turrets[Random.Range(0, turrets.Count)];
                turret.LookAt(currentEnemy.transform.position);
                GameObject bulletCopy = (GameObject)Instantiate(bulletPrefab, turret.position, turret.rotation);
                bulletCopy.GetComponent<LaserMover>().targetTag = currentEnemy.tag;
            }
            // Check the status of the current battle
            if (isFighting && currentEnemy != null) {
                float distance = Vector3.Distance(transform.position, currentEnemy.transform.position);
                if (distance > 150.0f) {
                    EndBattle();
                }
            }


            //// Check if the ship is still beeing chased if it is suppossed to be
            //if (!isCaptain && currentEnemy != null && state.Description() == "Patrolling/Defending") {
            //    float distance = Vector3.Distance(transform.position, currentEnemy.transform.position);
            //    if (distance > 100.0f) {
            //        EndBattle();
            //        SwitchState(new FormationFollowState(this));
            //    }
            //}
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
        if (gameObject.tag == "Raider") {
            Gizmos.color = new Color(1, 0, 1, 0.5f);
            Gizmos.DrawWireSphere(transform.position, 10);
        }
        if (gameObject.tag == "Viper") {
            Gizmos.color = new Color(0, 1, 1, 0.5f);
            Gizmos.DrawWireSphere(transform.position, 10);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (currentEnemy == null && other.GetType() == typeof(SphereCollider)) {
            if (other.gameObject.tag == "Viper" && other.gameObject.tag != gameObject.tag) {
                if (!other.gameObject.GetComponent<FighterStateMachine>().isFighting) {
                    PickBattle(other.gameObject);
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
