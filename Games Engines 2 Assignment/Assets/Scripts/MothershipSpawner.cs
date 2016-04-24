using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MothershipSpawner : MonoBehaviour {

    public GameObject spawnPrefab;
    public GameObject fighterBullet;
    List<Transform> spawns;
    List<Team> teams;
    
    [Range(0, 6)]
    public int numberOfTeams;
    [Range(0, 10)]
    public int teamSize;
    public int spawnTime = 4;
    public float formationOffset = 10;

    Stack<GameObject> toSpawn;

    // Use this for initialization
    void Start() {
        teams = new List<Team>();
        toSpawn = new Stack<GameObject>();

        // Get spawn points
        spawns = new List<Transform>();
        foreach (Transform child in transform.FindChild("SpawnPoints")) {
            if (child.tag == "SpawnPoint") {
                spawns.Add(child);
            }
        }
        // Setup all teams
        SetupTeams(numberOfTeams, teamSize);
        // Start spawning
        StartCoroutine("SpawnShips");
    }

    IEnumerator SpawnShips() {
        while (true) {
            if (toSpawn.Count != 0) {
                GameObject newSpawn = toSpawn.Pop();
                newSpawn.SetActive(true);
                Transform spawn = spawns[Random.Range(0, spawns.Count)];
                newSpawn.transform.position = spawn.position;
                newSpawn.transform.rotation = spawn.rotation;
            }

            yield return new WaitForSeconds(spawnTime);
        }
    }

    void SetupTeams(int numberOfTeams, int sizeOfTeams) {
        GameObject teamList = new GameObject("Teams");
        for (int i = 0; i < numberOfTeams; i++) {
            GameObject teamObject = new GameObject("Team " + i);
            Team newTeam = new Team(sizeOfTeams, formationOffset, spawns[Random.Range(0, spawns.Count)],
                spawnPrefab, fighterBullet,this.gameObject);

            foreach (GameObject squadmate in newTeam.squadmates) {
                toSpawn.Push(squadmate);
                squadmate.transform.parent = teamObject.transform;
            }
            toSpawn.Push(newTeam.captain);
            newTeam.captain.transform.parent = teamObject.transform;
            teams.Add(newTeam);

            teamObject.transform.parent = teamList.transform;
        }
    }

    public void Respawn(GameObject respawnObject) {
        toSpawn.Push(respawnObject);
    }

    public void ChangePatrolShip(GameObject target) {
        foreach (Team team in teams) {
            team.captain.GetComponent<FighterStateMachine>().motherShip = target;
            foreach (GameObject squadmate in team.squadmates) {
                squadmate.GetComponent<FighterStateMachine>().motherShip = target;
            }
        }
    }
}

