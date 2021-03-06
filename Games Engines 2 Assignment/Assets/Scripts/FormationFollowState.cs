﻿using UnityEngine;
using System.Collections;
using System;

public class FormationFollowState : State {
    public FormationFollowState(FighterStateMachine owner) : base(owner) {
    }

    public override string Description() {
        return "Following Formation";
    }

    public override void Enter() {
        owner.GetComponent<Boid>().formationLeader = owner.captainShip;
        owner.GetComponent<Boid>().formationOffset = owner.formationSpot;
        owner.GetComponent<Boid>().formationFollowingEnabled = true;
        owner.GetComponent<Boid>().maxSpeed += 20;
    }

    public override void Exit() {
        owner.GetComponent<Boid>().formationFollowingEnabled = false;
        owner.GetComponent<Boid>().maxSpeed -= 20;
    }

    public override void Update() {
    }
}
