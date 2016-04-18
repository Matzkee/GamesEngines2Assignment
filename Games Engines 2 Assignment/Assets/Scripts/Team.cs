using UnityEngine;
using System.Collections.Generic;

public class Team {

    public GameObject captain;
    public List<GameObject> squadmates;

    List<Vector3> formation;
    GameObject prefab, mothership;
    int squadSize;
    float formationOffset;
    Transform spawnPoint;

    // Contructor
    public Team(int _squadSize, float _formationOffset, Transform _spawnpoint, GameObject _prefab, GameObject _mothership) {
        squadSize = _squadSize;
        formationOffset = _formationOffset;
        spawnPoint = _spawnpoint;
        prefab = _prefab;
        mothership = _mothership;

        squadmates = new List<GameObject>();
        MakePyramidFormation();
        MakeTeam();
    }

    void MakeTeam() {
        // Add the squad captain
        captain = (GameObject)GameObject.Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        FSM captainMachine = captain.AddComponent<FSM>();
        captainMachine.isCaptain = true;
        captainMachine.health = 30;
        captainMachine.motherShip = mothership.transform;
        // set inactive for now so that mothership can begin spawning
        captain.name = "Team Captain";
        captain.SetActive(false);

        for (int i = 1; i < squadSize; i++) {
            GameObject squadmate = (GameObject)GameObject.Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            FSM squadMachine = squadmate.AddComponent<FSM>();
            squadMachine.motherShip = mothership.transform;
            squadMachine.formationSpot = formation[i - 1];
            squadMachine.captainShip = captain;
            squadmate.name = "Squadmate " + i;
            squadmates.Add(squadmate);
            squadmate.SetActive(false);
        }
    }

    public void MakePyramidFormation() {
        formation = new List<Vector3>();
        int rowCount = 1;

        while (formation.Count < squadSize) {
            Vector3 nextRow = new Vector3(-formationOffset * rowCount, 0, -formationOffset * rowCount);
            if (formation.Count < squadSize)
                formation.Add(nextRow);
            else
                break;
            Vector3 currentOffset = new Vector3(formationOffset * 2, 0, 0);
            for (int i = 1; i <= rowCount; i++) {
                if (formation.Count < squadSize)
                    formation.Add(nextRow + (currentOffset * i));
                else
                    break;
            }

            rowCount++;
        }
    }
}
