﻿using UnityEngine;
using System.Collections.Generic;

public class BattlePicker : MonoBehaviour {

    public int currentBattles;

    // This class serves as a battle picker for smaller fighters
    // The class dictates which ship is to flee and which to attack in a battle scenario
    // since both of them use same collider detection this is encessary
    HashSet<GameObject> battlingFighters;

    void Start() {
        battlingFighters = new HashSet<GameObject>();
    }

    public void PickFighterBattle(GameObject fighter1, GameObject fighter2) {
        if (!battlingFighters.Contains(fighter1) || !battlingFighters.Contains(fighter2)) {
            battlingFighters.Add(fighter1);
            battlingFighters.Add(fighter2);

            fighter1.GetComponent<FighterStateMachine>().currentEnemy = fighter2;
            fighter2.GetComponent<FighterStateMachine>().currentEnemy = fighter1;
            fighter1.GetComponent<FighterStateMachine>().isFighting = true;
            fighter2.GetComponent<FighterStateMachine>().isFighting = true;

            float random = Random.value;
            if (random > 0.5f) {
                fighter1.GetComponent<FighterStateMachine>().
                    SwitchState(new FightingState(fighter1.GetComponent<FighterStateMachine>()));
                fighter2.GetComponent<FighterStateMachine>().
                    SwitchState(new PatrollingState(fighter2.GetComponent<FighterStateMachine>()));
            }
            else {
                fighter2.GetComponent<FighterStateMachine>().
                    SwitchState(new FightingState(fighter2.GetComponent<FighterStateMachine>()));
                fighter1.GetComponent<FighterStateMachine>().
                    SwitchState(new PatrollingState(fighter1.GetComponent<FighterStateMachine>()));
            }
            currentBattles += 1;
        }
    }

    public void RemoveBattle(GameObject fighter1, GameObject fighter2) {
        if (battlingFighters.Contains(fighter1) || battlingFighters.Contains(fighter2)) {
            fighter1.GetComponent<FighterStateMachine>().isFighting = false;
            fighter2.GetComponent<FighterStateMachine>().isFighting = false;
            battlingFighters.Remove(fighter1);
            battlingFighters.Remove(fighter2);
            currentBattles -= 1;
        }
    }
}