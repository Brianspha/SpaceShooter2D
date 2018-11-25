using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour {
    public float Speed = 5.1f;
    Transform Player;
    Vector2 target;
    public EnemySpawner Internal;
    public ParticleSystem splat;
    public GameObject Rocket;
    public int RocketCount = 10;
    public bool spawnedRocket = false;
    Transform BossObject;
    bool BossTime;
    void Start () {
        //  Internal = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawner>();
        if (GameObject.FindGameObjectWithTag ("Enemy"))
            Player = GameObject.FindGameObjectWithTag ("Enemy").transform;
        target = new Vector2 (Player.position.x, Player.position.y);
    }
    private void Update () {
        transform.position = Vector2.MoveTowards (transform.position, new Vector2 (transform.position.x + Time.deltaTime * Speed, transform.position.y), Time.deltaTime * Speed);
        if (this.transform.position.x == target.x && this.transform.position.y == target.y || this.transform.position.x >= target.x) {
            DestroyBullet ();
        }

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
                GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().shakeAmount = 5f;
                GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().shakeDuration = 3f;
                GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().ShakeCamera = true;
                Destroy (other.gameObject); //@dev add player death splat        
            } else {
                if (other.gameObject.GetComponent<BeforeBossEnemy> ()) {
                    other.gameObject.GetComponent<BeforeBossEnemy> ().Information.Health--;
                }
                if (other.gameObject.GetComponent<BossAlie> ()) {
                    other.gameObject.GetComponent<BossAlie> ().Information.Health--;
                }
                if (other.gameObject.GetComponent<Boss> () && !other.gameObject.GetComponent<Boss> ().AliesDead) {
                    other.gameObject.GetComponent<Boss> ().Information.Health--;
                }
                if (other.gameObject.GetComponent<Enemy> ()) {
                    other.gameObject.GetComponent<Enemy> ().Information.Health--;
                }
            }
        }
        if (other.gameObject.CompareTag ("BulletEnemy")) {
            Splat ();
            if (!other.gameObject.GetComponent<Bullet> ().SpecialBullet || other.gameObject.GetComponent<Bullet> ().Health <= 0) Destroy (other.gameObject);
            else {
                DestroyBullet ();
                other.gameObject.GetComponent<Bullet> ().Health--;
            }
            GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().Score += 1;
        }
    }

    /**
     *@dev Destroys the bullet 
     */
    void DestroyBullet () {
        Destroy (this.gameObject);
    }

    /**
     *@dev instantiates splat effect
     */
    public void Splat () {
        Instantiate (splat, this.transform.position, Quaternion.identity);
    }
    void SpwanRocket () {
        Instantiate (Rocket, this.transform.position, Quaternion.identity);
    }
}