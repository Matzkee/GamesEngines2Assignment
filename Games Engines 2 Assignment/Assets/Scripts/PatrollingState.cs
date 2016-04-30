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
        Vector3 patrolPoint = PickNewWaypoint();
        Path path = MakePath(patrolPoint);
        owner.GetComponent<Boid>().path = path;
        owner.GetComponent<Boid>().pathFollowingEnabled = true;
    }

    public override void Exit() {
        owner.GetComponent<Boid>().pathFollowingEnabled = false;
    }

    public override void Update() {
        if (owner.GetComponent<Boid>().path != null) {
            if (owner.GetComponent<Boid>().path.reachedEnd) {
                Vector3 patrolPoint = PickNewWaypoint();
                Path path = MakePath(patrolPoint);
                owner.GetComponent<Boid>().path = path;
            }
        }
    }

    Vector3 PickNewWaypoint() {
        if (owner.motherShip != null) {
            return (owner.motherShip.transform.position + (Random.insideUnitSphere * owner.patrolRadius));
        }
        else {
            return owner.transform.position;
        }
    }

    Path MakePath(Vector3 target) {
        if (owner.motherShip != null) {
            if (!owner.motherShip.GetComponent<Collider>().bounds.Contains(owner.transform.position)) {
                RaycastHit[] hits;
                List<Collider> obstacles = new List<Collider>();
                Vector3 toTarget = (target - owner.transform.position).normalized;
                float dist = Vector3.Distance(owner.transform.position, target);
                hits = Physics.RaycastAll(owner.transform.position, toTarget, dist);

                for (int i = 0; i < hits.Length; i++) {
                    RaycastHit hit = hits[i];
                    if (hit.collider.gameObject.tag == "Pegasus") {
                        obstacles.Add(hit.collider);
                    }
                }
                // Check if any obstacles were found
                if (obstacles.Count > 0 && dist > owner.voxelSize) {
                    return owner.pathfinder.FindPath(owner.transform.position, target, obstacles);
                }
            }
        }
        // otherwise return the patrol point as path
        Path path = new Path();
        path.waypoints.Add(target);
        return path;
    }
}