using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Boss : MonoBehaviour {

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
    public Vector2 Original, Current;
    public float MoveDistanceForSquare = 6f;
    // Use this for initialization
    public Vector2[] Points;
    public GameObject[] Alies;
    public float MaxTimeToSendAllie = 5f;
    public float CurrentTimeSinceAllieSent = 0f;
    public bool SentAlie = false;
    public float TimeSinceCameIntoScene = 10f;
    public GameObject CurrentAlie;
    public GameObject CurrentAlieOriginalTransform;

    public Vector2 To;
    public float moveDistStart = 2f;
    public float MaxTimeToReturnToOriginal = 6f;
    public float currentMaxTimeToReturnToOriginal = 0f;
    public float MoveDistforAlie = 5f;
    public bool AliesDead = false;
    public bool AllieGoBack = false;
    public bool AlreadyLaunchedAlie = false;
    public bool AlreadyCheckedifAlive = false;
    public Vector2 CurrentBossGoToPos;
    public GameObject[] BossBullets;
    public bool AllDead = false;
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
        Original = transform.position;
        Vector2 A = GameObject.FindGameObjectWithTag ("PointA").transform.position;
        Vector2 B = GameObject.FindGameObjectWithTag ("PointB").transform.position;
        Points = new Vector2[] { A, B };
        Current = Points[Random.Range (0, Points.Length)];
        currentMaxTimeToReturnToOriginal = MaxTimeToReturnToOriginal;

    }

    void Update () {
        if (!AliesDead) {
            if (CurrentAlie && !AllieGoBack) {
                CurrentAlie.transform.position = Vector2.MoveTowards (CurrentAlie.transform.position, Current, Time.deltaTime * Speed);
                Debug.Log ("Current= " + CurrentAlie.transform.position);
                // CurrentAlie=CurrentAlie;

            } else {
                //  SendAlie();
            }
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

            if (!AlreadyLaunchedAlie && TimeSinceCameIntoScene <= 0) {
                SentAlie = true;
                SendAlie ();
                AlreadyLaunchedAlie = true;
            }

            if (!AlreadyCheckedifAlive && CheckIfAlieDead ()) {
                Debug.Log ("!AlreadyCheckedifAlive&&CheckIfAlieDead()= " + currentMaxTimeToReturnToOriginal);
                AlreadyCheckedifAlive = true;
                SendAlie ();
                CurrentTimeSinceAllieSent = MaxTimeToSendAllie;
            }
            if (!SentAlie) {
                TimeSinceCameIntoScene -= Time.deltaTime;
            }
            if (SentAlie) {
                CurrentTimeSinceAllieSent -= Time.deltaTime;
                currentMaxTimeToReturnToOriginal -= Time.deltaTime;
            }
            if (!AllieGoBack && currentMaxTimeToReturnToOriginal <= 0) {
                AllieGoBack = true;
            }
            if (AllieGoBack) {
                if (CurrentAlie) CurrentAlie.transform.position = Vector2.MoveTowards (CurrentAlie.transform.transform.position, CurrentAlieOriginalTransform.transform.position, MoveDistforAlie * Time.deltaTime);
                else {
                    SendAlie ();
                    AllieGoBack = false;
                    currentMaxTimeToReturnToOriginal = MaxTimeToReturnToOriginal;
                    AlreadyCheckedifAlive = false;
                }
                if (CurrentAlie && CurrentAlie.transform.position.x == CurrentAlieOriginalTransform.transform.position.x && CurrentAlie.transform.position.y == CurrentAlieOriginalTransform.transform.position.y) {
                    Debug.Log ("InAllieGoBack= " + currentMaxTimeToReturnToOriginal);
                    SendAlie ();
                    AllieGoBack = false;
                    currentMaxTimeToReturnToOriginal = MaxTimeToReturnToOriginal;
                    AlreadyCheckedifAlive = false;
                }
            }

            TimeBetweenBullets -= Time.deltaTime;
        } else {
            transform.position = Vector2.MoveTowards (transform.position, CurrentBossGoToPos, moveDistStart * Time.deltaTime);
            if (transform.position.x == CurrentBossGoToPos.x && transform.position.x == CurrentBossGoToPos.x) {
                CurrentBossGoToPos = Points[Random.Range (0, Points.Length)];
            }
            //@dev ensure that we only spawn bullets when we infront of the player not when we've passed him/her
            if (TimeBetweenBullets <= 0 && transform.position.x >= player.transform.position.x) {
                SpawnBullet ();
                TimeBetweenBullets = BulletLaunchMaxTime; //reset launch time
            }
            TimeBetweenBullets -= Time.deltaTime;
        }
    }

    /*
     *@dev Sends an alie towards player
     */
    void SendAlie () {
        int destroyedCount = 0;
        for (int i = 0; i < Alies.Length; i++) {
            if (!Alies[i]) destroyedCount++;
        }
        if (Alies.Length == 0 || destroyedCount == Alies.Length) {
            AliesDead = true;
            AddPointsForBossMovement ();
            return;
        }
        CurrentAlie = Alies[Random.Range (0, Alies.Length)];
        if (CurrentAlie) {
            CurrentAlieOriginalTransform = CurrentAlie;
            CurrentAlieOriginalTransform.transform.position = CurrentAlie.GetComponent<BossAlie> ().OriginalPosition.position;
        }
    }
    public void AddPointsForBossMovement () {
        var temp = Points.ToList ();
        temp.Add (GameObject.FindGameObjectWithTag ("PointC").transform.position);
        temp.Add (GameObject.FindGameObjectWithTag ("PointD").transform.position);
        Points = temp.ToArray ();
        CurrentBossGoToPos = Points[Random.Range (0, Points.Length)];
        Information.Health += Random.Range (100, 150); //@dev make boss even stronger
    }
    /*
     *@dev checks if an alie is dead
     */
    public void RemoveAlie (GameObject allie) {
        var temp = Alies.ToList ();
        temp.Remove (allie);
        Alies = temp.ToArray ();
    }
    public bool CheckIfAlieDead () {
        bool dead = false;
        if (Alies.Length == 0) {
            AliesDead = true;
            AddPointsForBossMovement ();
            return AliesDead;
        }
        for (int i = 0; i < Alies.Length; i++) {
            if (!Alies[i]) {
                dead = true;
                break;
            }
            var current = Alies[i].GetComponent<BossAlie> ().Information;
            if (current.Health < 0) {
                dead = true;
                break;
            }
        }
        return dead;
    }
    /*
     *@dev Spawns a bullet
     */
    void SpawnBullet () {
        if (!AliesDead)
            Instantiate (bullet, this.transform.position, Quaternion.identity);
        else
            Instantiate (BossBullets[Random.Range (0, BossBullets.Length)], transform.position, Quaternion.identity);
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
            health = Random.Range (150, 250);
        }
        if (index > 2 && index < 5) {
            health = Random.Range (100, 150);
        }
        if (index == 0) health = 1;
        else if (index <= 2) {
            health = Random.Range (50, 100);
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