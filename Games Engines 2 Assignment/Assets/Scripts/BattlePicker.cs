using UnityEngine;
using System.Collections.Generic;

public class BattlePicker : MonoBehaviour {

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

            float random = Random.value;
            if (random > 0.5f) {
                fighter1.GetComponent<FighterStateMachine>().isFighting = true;
                fighter2.GetComponent<FighterStateMachine>().isFighting = true;
                fighter1.GetComponent<FighterStateMachine>().isAttacking = true;
                fighter2.GetComponent<FighterStateMachine>().isAttacking = false;
            }
        }
    }

    public void RemoveFighter(GameObject fighter) {
        if (battlingFighters.Contains(fighter)) {
            battlingFighters.Remove(fighter);
        }
    }
}
