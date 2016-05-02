using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PatrollingState : State {

    public PatrollingState(FighterStateMachine owner) : base(owner) {
    }

    public override string Description() {
        return "Patrolling/Defending";
    }

    public override void Enter() {
        // Pick a new waypoint & find a path to it
        Path path = MakePath();
        owner.GetComponent<Boid>().path = path;
        owner.GetComponent<Boid>().pathFollowingEnabled = true;
    }

    public override void Exit() {
        owner.GetComponent<Boid>().pathFollowingEnabled = false;
    }

    public override void Update() {
        if (owner.GetComponent<Boid>().path != null) {
            if (owner.GetComponent<Boid>().path.reachedEnd) {
                Path path = MakePath();
                owner.GetComponent<Boid>().path = path;
            }
        }
    }

    Path MakePath() {
        // Make the patrol point in a cyllinder above the mothership
        Vector3 patrolPoint;
        if (owner.patrolShip != null) {
            patrolPoint = owner.patrolShip.transform.position + (Random.insideUnitSphere * owner.patrolRadius);
            patrolPoint.y = owner.patrolShip.transform.position.y + 300 + Random.Range(0, 200);
        }
        else {
            patrolPoint =  owner.transform.position;
        }

        Path path = new Path();
        path.waypoints.Add(patrolPoint);
        return path;
    }
}