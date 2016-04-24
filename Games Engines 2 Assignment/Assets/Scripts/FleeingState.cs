using UnityEngine;
using System.Collections;
using System;

public class FleeingState : State {

    public FleeingState(FighterStateMachine owner) : base(owner) {
    }

    public override string Description() {
        return "Fleeing from enemy";
    }

    public override void Enter() {
        owner.GetComponent<Boid>().fleeRange = 80;
        owner.GetComponent<Boid>().fleeTargetPos = owner.currentEnemy.transform.position;
        owner.GetComponent<Boid>().fleeEnabled = true;
    }

    public override void Exit() {
        throw new NotImplementedException();
    }

    public override void Update() {
        float distance = Vector3.Distance(owner.transform.position, owner.currentEnemy.transform.position);
        if (distance > 110) {
            owner.EndBattle();
            owner.SwitchState(new FormationFollowState(owner));
        }
        owner.GetComponent<Boid>().fleeTargetPos = owner.currentEnemy.transform.position;
    }
}
