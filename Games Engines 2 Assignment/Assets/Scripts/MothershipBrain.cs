using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MothershipBrain : MonoBehaviour {

    public int health = 200;
    public GameObject enemy;
    public bool isStationery = true;

    [Header("Shooting options")]
    public float precisionFactor = 50;
    public float minAttackDelay = 2;
    public float maxAttackDelay = 5;
    public float bulletSpeed = 1000;
    public GameObject bullet;

    List<Transform> turrets;

    void Start() {
        StartCoroutine("Setup");
    }

    IEnumerator Setup() {
        // Wait for the mothership spawner to setup teams & begin the fight
        while (GetComponent<MothershipSpawner>().teamsReady != true) {
            yield return new WaitForSeconds(2);
        }
        // Get positions of turrets
        turrets = new List<Transform>();
        foreach (Transform child in transform.FindChild("Turrets")) {
            turrets.Add(child);
        }
        // Pick a point at a certain distance if the ship is approaching
        if (!isStationery) {
            Vector3 toTarget = (enemy.transform.position - transform.position).normalized;
            Vector3 approachPoint = enemy.transform.position - (toTarget * 1500);
            GetComponent<Boid>().arriveTargetPos = approachPoint;
            GetComponent<Boid>().arriveEnabled = true;
        }
        else {
            GetComponent<MothershipSpawner>().ChangePatrolShip(enemy);
        }

        StartCoroutine("Shoot");
        StartCoroutine("Fight");
    }

    IEnumerator Fight() {
        while (health > 0) {
            // Check every 5 sec if the ship is destroyed
            yield return new WaitForSeconds(5);
        }
        // Respawn again 
        gameObject.SetActive(false);
    }

    IEnumerator Shoot() {
        while (enemy != null) {

            // Shot in enemy direction with a precision factor
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            Vector3 shootTarget = Random.insideUnitSphere * (distance / precisionFactor);
            Transform turret = turrets[Random.Range(0,turrets.Count)];
            turret.LookAt(enemy.transform.position + shootTarget);
            GameObject bulletCopy = (GameObject) Instantiate(bullet, turret.position, turret.rotation);
            bulletCopy.GetComponent<LaserMover>().targetTag = enemy.tag;

            yield return new WaitForSeconds(Random.Range(minAttackDelay, maxAttackDelay));
        }
    }

}
