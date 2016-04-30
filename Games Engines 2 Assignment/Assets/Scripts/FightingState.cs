using UnityEngine;
using System.Collections;
using System;

public class FightingState : State {
    public FightingState(FighterStateMachine owner) : base(owner) {
    }

    public override string Description() {
        return "Fighting";
    }

    public override void Enter() {
        owner.attackingTarget = true;
        owner.GetComponent<Boid>().pursueTarget = owner.currentEnemy;
        owner.GetComponent<Boid>().pursueEnabled = true;
    }

    public override void Exit() {
        owner.attackingTarget = false;
        owner.GetComponent<Boid>().pursueEnabled = false;
    }

    public override void Update() {
    }
}
