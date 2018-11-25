using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class BossAlie : MonoBehaviour {

    public int Health { get; set; }
    public int Type { get; set; }
    public HelperFunctions.EnemyInformation Information;
    private GameObject player;
    public float MinDistance = 10.5f;
    public float Speed = 5f;
    public float runAwayDistance = 5f;
    public float stopDistance = 5f;
    public float MinDistanceBetween = 0.5f;
    public float TimeBetweenBullets = 0f;
    public float BulletLaunchMaxTime = 1f;
    public GameObject bullet;
    public float MoveDistanceForSquare = 6f;
    public bool Move = true;
    public Vector2 To;
    public float moveDistStart = 2f;
    public Transform OriginalPosition;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag ("Player");
        this.gameObject.GetComponent<Rigidbody2D> ().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        this.gameObject.AddComponent<CircleCollider2D> ();
        this.gameObject.GetComponent<CircleCollider2D> ().isTrigger = true;
        SpriteRenderer temp = this.gameObject.GetComponent<SpriteRenderer> ();
        int index = Random.Range (0, HelperFunctions.Available.Length);
        temp.color = HelperFunctions.Available[index];
        To = new Vector2 (transform.position.x + moveDistStart, transform.position.y);
        Information = new HelperFunctions.EnemyInformation { Color = temp, Health = GenerateHealth (index), type = GetType (index) };
        gameObject.GetComponent<SpriteRenderer> ().color = HelperFunctions.Available[index];
        OriginalPosition = transform;
        //transform.position=To*Time.deltaTime;	
    }

    void Update () {
        //@dev ensure that we only spawn bullets when we infront of the player not when we've passed him/her
        //@dev ensure that we only spawn bullets when we infront of the player not when we've passed him/her
        if (TimeBetweenBullets <= 0 && transform.position.x >= player.transform.position.x) {
            SpawnBullet ();
            TimeBetweenBullets = BulletLaunchMaxTime; //reset launch time
        } else if (transform.position.x <= player.transform.position.x) {
            DestroyObject ();
            TimeBetweenBullets = BulletLaunchMaxTime; //reset launch time
        }
        TimeBetweenBullets -= Time.deltaTime;
    }

    void SpawnBullet () {
        Instantiate (bullet, this.transform.position, Quaternion.identity);
    }

    /*
     *@dev Gets health based of index
     *@param index used to determine the health the enemy will have
     */
    public int GenerateHealth (int index) {
        int health = 0;
        if (index == 5) {
            health = Random.Range (0, 120);
        }
        if (index > 2 && index < 5) {
            health = Random.Range (0, 60);
        }
        if (index == 0) health = 1;
        else if (index <= 2) {
            health = Random.Range (0, 30);
        }
        return health;
    }

    /**
     *@dev gets the type to be assigned to the enemy
     *@param index used to determine the type to be assigned to enemy
     */
    public HelperFunctions.EnemyType GetType (int index) {
        var TypeE = HelperFunctions.EnemyType.Normal;
        if (index == 5) TypeE = HelperFunctions.EnemyType.Advance;
        if (index >= 2 && index < 5) TypeE = HelperFunctions.EnemyType.Intermediate;
        return TypeE;
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
    void DestroyObject () {
        Destroy (this.gameObject);
    }

}