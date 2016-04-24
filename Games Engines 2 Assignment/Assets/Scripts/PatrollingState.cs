using UnityEngine;
using System.Collections;


public class PatrollingState : State {

    public PatrollingState(FighterStateMachine owner) : base(owner) {
    }

    public override string Description() {
        return "Patrolling/Defending";
    }

    public override void Enter() {
        if (owner.motherShip != null) {
            owner.GetComponent<Boid>().patrolTransform = owner.motherShip.transform;
        }
        else {
            owner.GetComponent<Boid>().patrolTransform = owner.transform;
        }
        owner.GetComponent<Boid>().patrolRadius = owner.patrolRadius;
        owner.GetComponent<Boid>().patrolEnabled = true;
    }

    public override void Exit() {
        owner.GetComponent<Boid>().patrolEnabled = false;
    }

    public override void Update() {
    }
}