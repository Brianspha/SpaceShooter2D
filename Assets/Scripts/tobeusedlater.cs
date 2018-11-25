using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TobeUsed : MonoBehaviour {

    public int Health { get; private set; }
    public int Type { get; private set; }
    private GameObject player;
    public float MinDistance = 10.5f;
    public float Speed = 5f;
    public float runAwayDistance = 5f;
    public float stopDistance = 5f;
    public float MinDistanceBetween = 0.5f;
    public float TimeBetweenBullets = 0f;
    public float BulletLaunchMaxTime = 5f;
    public GameObject bullet;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag ("Player");
    }

    void Update () {
        if (Vector2.Distance (transform.position, player.transform.position) > MinDistance) {
            this.transform.position = Vector2.MoveTowards (transform.position, player.transform.position, Speed * Time.deltaTime);
        } else if (Vector2.Distance (transform.position, player.transform.position) < runAwayDistance) {
            this.transform.position = Vector2.MoveTowards (transform.position, player.transform.position, -Speed * Time.deltaTime);
        } else {
            transform.position = this.transform.position;
        }
        if (TimeBetweenBullets <= 0) {
            SpawnBullet ();
            TimeBetweenBullets = BulletLaunchMaxTime; //reset launch time
        }
        TimeBetweenBullets -= Time.deltaTime;
    }
    void SpawnBullet () {
        Instantiate (bullet, this.transform.position, Quaternion.identity);
    }
    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate () {

    }
    /*
    *@dev function responsible for decreasing the health of the enemy (current) using 
     the the hitcount varibale
     @param hitcount specifies the extent to which the enemy was hit or rather how hard the player colided
     with the bullet fired by player
     */
    public void decreaseHealth (int hitcount) {
        Health--;
    }
    /**
    @dev function returns true if player health has reached 0 false otherwise 
     */
    public bool isDead () {
        return Health <= 0;
    }

}