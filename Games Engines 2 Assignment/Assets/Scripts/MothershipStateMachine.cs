﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MothershipStateMachine : MonoBehaviour {

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

    State state = null;

    void Start() {
        // Get positions of turrets
        turrets = new List<Transform>();
        foreach (Transform child in transform.FindChild("Turrets")) {
            turrets.Add(child);
        }
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
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            Vector3 shootTarget = Random.insideUnitSphere * (distance / precisionFactor);
            Transform turret = turrets[Random.Range(0,turrets.Count)];
            turret.LookAt(enemy.transform.position + shootTarget);
            Instantiate(bullet, turret.position, turret.rotation);

            yield return new WaitForSeconds(Random.Range(minAttackDelay, maxAttackDelay));
        }
    }

}
