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
        owner.GetComponent<Boid>().fleeRange = 110;
        owner.GetComponent<Boid>().fleeTargetPos = owner.currentEnemy.transform.position;
        owner.GetComponent<Boid>().fleeEnabled = true;
    }

    public override void Exit() {
        owner.GetComponent<Boid>().fleeEnabled = false;
    }

    public override void Update() {
        if (owner.currentEnemy != null) {
            float distance = Vector3.Distance(owner.transform.position, owner.currentEnemy.transform.position);
            owner.GetComponent<Boid>().fleeTargetPos = owner.currentEnemy.transform.position;

            if (distance > 110) {
                owner.EndBattle();
                if (owner.isCaptain) {
                    owner.SwitchState(new PatrollingState(owner));
                }
                else {
                    owner.SwitchState(new FormationFollowState(owner));
                }
            }
        }
        else {
            owner.EndBattle();
            if (owner.isCaptain) {
                owner.SwitchState(new PatrollingState(owner));
            }
            else {
                owner.SwitchState(new FormationFollowState(owner));
            }
        }
    }
}
