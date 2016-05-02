using UnityEngine;
using System.Collections.Generic;

public class Team {

    public GameObject captain;
    public List<GameObject> squadmates;

    List<Vector3> formation;
    GameObject prefab, mothership, bullet;
    int squadSize;
    float formationOffset;
    Transform spawnPoint;

    // Contructor
    public Team(int _squadSize, float _formationOffset, Transform _spawnpoint, GameObject _prefab, GameObject _bullet, GameObject _mothership) {
        squadSize = _squadSize;
        formationOffset = _formationOffset;
        spawnPoint = _spawnpoint;
        prefab = _prefab;
        mothership = _mothership;
        bullet = _bullet;

        squadmates = new List<GameObject>();
        MakePyramidFormation();
        MakeTeam();
    }

    void MakeTeam() {
        // Add the squad captain
        captain = (GameObject)GameObject.Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        FighterStateMachine captainMachine = captain.AddComponent<FighterStateMachine>();
        captainMachine.isCaptain = true;
        captainMachine.health = 30;
        captainMachine.spawnShip = mothership;
        captainMachine.bulletPrefab = bullet;
        // set inactive for now so that mothership can begin spawning
        captain.name = "Team Captain";
        captain.SetActive(false);

        for (int i = 1; i < squadSize; i++) {
            GameObject squadmate = (GameObject)GameObject.Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            FighterStateMachine squadMachine = squadmate.AddComponent<FighterStateMachine>();
            squadMachine.spawnShip = mothership;
            squadMachine.formationSpot = formation[i - 1];
            squadMachine.captainShip = captain;
            squadMachine.bulletPrefab = bullet;
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
