using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class BeforeBossEnemy : MonoBehaviour {

    public int Health = 0;
    public int Type;
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
    public Vector2 PA, PB, PC, Current;
    public float MoveDistanceForSquare = 6f;
    // Use this for initialization
    public Vector2[] Points;
    void Start () {
        player = GameObject.FindGameObjectWithTag ("Player");
        this.gameObject.GetComponent<Rigidbody2D> ().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        this.gameObject.AddComponent<CircleCollider2D> ();
        this.gameObject.GetComponent<CircleCollider2D> ().isTrigger = true;
        SpriteRenderer temp = this.gameObject.GetComponent<SpriteRenderer> ();
        int index = Random.Range (0, HelperFunctions.Available.Length);
        temp.color = HelperFunctions.Available[index];
        Information = new HelperFunctions.EnemyInformation { Color = temp, Health = GenerateHealth (index), type = GetType (index) };
        gameObject.GetComponent<SpriteRenderer> ().color = HelperFunctions.Available[index];
        PA = GameObject.FindGameObjectWithTag ("PointA").transform.position;
        PB = GameObject.FindGameObjectWithTag ("PointB").transform.position;
        PC = GameObject.FindGameObjectWithTag ("PointC").transform.position;
        Points = new Vector2[] { PA, PB, PC };
        Current = Points[Random.Range (0, Points.Length)];
    }

    void Update () {
        Health = Information.Health;
        transform.position = Vector2.MoveTowards (transform.position, Current, Time.deltaTime * Speed);
        if (Current.x == transform.position.x && Current.y == transform.position.y) {
            Current = Points[Random.Range (0, Points.Length)];
        }
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
    public void Reset () {

    }
    /*
     *@dev Gets health based of index
     *@param index used to determine the health the enemy will have
     */
    public int GenerateHealth (int index) {
        int health = 0;
        if (index == 5) {
            health = Random.Range (0, 100);
        }
        if (index > 2 && index < 5) {
            health = Random.Range (0, 40);
        }
        if (index == 0) health = 1;
        else if (index <= 2) {
            health = Random.Range (0, 20);
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