using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySpawner : MonoBehaviour {

    public float MinShootDistance = 8f;
    public GameObject BeforeBoss;
    public GameObject[] Enemies;
    public float CurrentTimeSinceSpawn = 0f;
    private Transform Player;
    public float MaxSpawnTimeBetweenEnemies = 8f;
    public GameObject Active;
    public float minDistanceToDestroy = 30f;
    public float TestDist = 0f;
    public int MinToSpawnDuringBoss = 3;
    // Use this for initialization
    public float Distance = 0f;
    public float MaxDistanceForBoss = 3000f;
    public int Acumalator = 2;
    public bool StopTime = false;
    public bool AlreadySpawnedBeforeBoss = false;
    public float MaxDistanceForBeforeBoss = 3000f;
    public bool BossTime = false;
    public GameObject[] Bosses;
    public GameObject[] Goodies;
    public float MaxTimeForGoodies = 20f;
    public float CurrentTimeForGoodies = 0;
    public bool SpawnedBoss=false;
    void Start () {
        Player = GameObject.FindGameObjectWithTag ("Player").transform;
        CurrentTimeForGoodies = MaxTimeForGoodies;
    }

    void Update () {
        Player = GameObject.FindGameObjectWithTag ("Player").transform;
        if (!BossTime && CurrentTimeSinceSpawn <= 0) {
            Spawn ();
            CurrentTimeSinceSpawn = MaxSpawnTimeBetweenEnemies;
        }
        if (Distance <= MaxDistanceForBeforeBoss) Distance += Time.deltaTime + Acumalator;;
        if (!AlreadySpawnedBeforeBoss && Distance >= MaxDistanceForBeforeBoss) {
            SpawnBeforeBoss ();
            AlreadySpawnedBeforeBoss = true;
            StopTime = true;
        }
        if (AlreadySpawnedBeforeBoss && BossTime &&!SpawnedBoss) {
            SpawnBoss ();
            SpawnedBoss=true;
        }
        if (!StopTime) {
            CurrentTimeSinceSpawn -= Time.deltaTime;
            CurrentTimeForGoodies -= Time.deltaTime;
        }

    }

    /*
    *@dev Spawns the random boss from the list of bosses
     
    */
    void SpawnBoss () {
        Instantiate (Bosses[Random.Range (0, Bosses.Length)], transform.position, Quaternion.identity);
    }

    /*
    @dev spawns a group of enemies based on the count passed in
     */
    public void Spawn () {
        var tempGameObject = Enemies[Random.Range (0, Enemies.Length)];
        tempGameObject.GetComponent<Enemy> ();
        Active = Instantiate (Enemies[Random.Range (0, Enemies.Length)], transform.position, Quaternion.identity) as GameObject;
        if (CurrentTimeForGoodies <= 0) {
            CurrentTimeForGoodies = MaxTimeForGoodies;
            //  Instantiate (Goodies[Random.Range (0, Goodies.Length)], this.transform.position, Quaternion.identity);
        }
    }
    /**
    @dev Resets spawn time
    */
    public void ResetTime () {
        CurrentTimeSinceSpawn = MaxSpawnTimeBetweenEnemies;
    }
    /**
     *@dev invoked when enemy has passed the player object
     */
    void DestroyEnemy (GameObject obj) {
        Destroy (obj);
    }

    /**
     *@dev spawn boss when certain distance is reached
     */
    public void SpawnBeforeBoss () {
        Instantiate (BeforeBoss, transform.position, Quaternion.identity);
    }

}