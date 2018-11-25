using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
    public float Speed = 5.1f;
    Transform Player;
    public EnemySpawner Internal;
    public ParticleSystem splat;
    public int RocketCount = 10;
    public bool spawnedRocket = false;
    Transform BossObject;
    public float Strenght = 7;
    bool alive = true;
    bool BossTime;
    public float aliveTime=6;
    void Start () {
        //  Internal = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawner>();
        if (GameObject.FindGameObjectWithTag ("Enemy"))
            Player = GameObject.FindGameObjectWithTag ("Enemy").transform;
        Player.position = new Vector2 (Player.position.x, Player.position.y);
    }
    private void Update () {
        if (alive) transform.position = Vector2.MoveTowards (transform.position, Player.position, Time.deltaTime * Speed);
        if (transform.position.x == Player.position.x && transform.position.y == Player.position.y || transform.position.x >= Player.position.x||aliveTime<=0) {
            DestroyBullet ();
        }
        aliveTime-=Time.deltaTime;
    }
    private void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.CompareTag ("Enemy")) {
            DestroyBullet ();
            GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().Score += 1;
            if (other.gameObject.GetComponent<Enemy> () && other.gameObject.GetComponent<Enemy> ().Information.Health <= 0) {
                Splat ();
                GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().ShakeCamera = true;
                Destroy (other.gameObject); //@dev add player death splat
            }
            if (other.gameObject.GetComponent<BeforeBossEnemy> () && other.gameObject.GetComponent<BeforeBossEnemy> ().Information.Health <= 0) {
                Splat ();
                GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().ShakeCamera = true;
                Destroy (other.gameObject); //@dev add player death splat     
                GameObject.FindGameObjectWithTag ("Spawner").GetComponent<EnemySpawner> ().BossTime = true;
            }
            if (other.gameObject.GetComponent<BossAlie> () && other.gameObject.GetComponent<BossAlie> ().Information.Health <= 0) {
                Splat ();
                GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().ShakeCamera = true;
                Destroy (other.gameObject); //@dev add player death splat        
            }
            if (other.gameObject.GetComponent<Boss> () && other.gameObject.GetComponent<Boss> ().Information.Health <= 0) {
                Splat ();
                GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().shakeAmount = 2f;
                GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().shakeDuration = 1f;
                GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().ShakeCamera = true;
                Destroy (other.gameObject); //@dev add player death splat        
            } else {
                if (other.gameObject.GetComponent<BeforeBossEnemy> ()) {
                    GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().shakeAmount = 2f;
                    GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().shakeDuration = 1f;
                    GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().ShakeCamera = true;
                    other.gameObject.GetComponent<BeforeBossEnemy> ().Information.Health -= int.Parse (Strenght.ToString ());
                    Strenght -= other.gameObject.GetComponent<BeforeBossEnemy> ().Health;

                }
                if (other.gameObject.GetComponent<BossAlie> ()) {
                    GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().shakeAmount = 2f;
                    GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().shakeDuration = 1f;
                    GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().ShakeCamera = true;
                    other.gameObject.GetComponent<BossAlie> ().Information.Health -= int.Parse (Strenght.ToString ());
                }
                if (other.gameObject.GetComponent<Boss> () && !other.gameObject.GetComponent<Boss> ().AliesDead) {
                    GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().shakeAmount = 2f;
                    GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().shakeDuration = 1f;
                    GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().ShakeCamera = true;
                    other.gameObject.GetComponent<Boss> ().Information.Health -= int.Parse (Strenght.ToString ());
                    Strenght -= other.gameObject.GetComponent<Boss> ().Health;

                }
                if (other.gameObject.GetComponent<Enemy> ()) {
                    GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().shakeAmount = 2f;
                    GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().shakeDuration = 1f;
                    GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().ShakeCamera = true;
                    other.gameObject.GetComponent<Enemy> ().Information.Health -= int.Parse (Strenght.ToString ());
                    Strenght -= other.gameObject.GetComponent<Enemy> ().Health;

                }
            }
        }
        if (other.gameObject.CompareTag ("BulletEnemy")) {
            Splat ();
            if (!other.gameObject.GetComponent<Bullet> ().SpecialBullet || other.gameObject.GetComponent<Bullet> ().Health <= 0) Destroy (other.gameObject);
            else {
                if (Strenght <= 0)
                    DestroyBullet ();
                other.gameObject.GetComponent<Bullet> ().Health -= Strenght;
                Strenght -= other.gameObject.GetComponent<Bullet> ().Health;
            }
            GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().Score += 1;
        }
    }

    /**
     *@dev Destroys the bullet 
     */
    void DestroyBullet () {
        alive = false;
        Destroy (this.gameObject);
    }

    /**
     *@dev instantiates splat effect
     */
    public void Splat () {
        Instantiate (splat, this.transform.position, Quaternion.identity);
    }

}