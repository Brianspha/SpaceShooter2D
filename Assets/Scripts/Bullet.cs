using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float Speed = 5.1f;
    Transform Player;
    Vector2 target;
    public EnemySpawner Internal;
    public bool SpecialBullet = false;
    public float Health;
    public int SpecialBulletDecreaser = 2;
    void Start () {
        Internal = GameObject.FindGameObjectWithTag ("Spawner").GetComponent<EnemySpawner> ();
        Player = GameObject.FindGameObjectWithTag ("Player").transform;
        target = new Vector2 (Player.position.x, this.transform.position.y);
        Health = Random.Range (5, 25);
    }
    private void Update () {
        if (SpecialBullet) transform.position = Vector2.MoveTowards (transform.position, Player.position, Time.deltaTime * Speed);
        else transform.position = Vector2.MoveTowards (transform.position, new Vector2 (target.x, target.y), Time.deltaTime * Speed);
        if ((transform.position.x == target.x && transform.position.y == target.y || transform.position.x < target.x) || (transform.position.x == Player.transform.position.x && transform.position.y == Player.transform.position.y || transform.position.x < Player.transform.position.x)) {
            DestroyBullet ();
        }
    }
    private void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.CompareTag ("Player")) {
            DestroyBullet ();
            if (!SpecialBullet)
                other.GetComponent<PlayerController> ().Health -= 1;
            else
                other.GetComponent<PlayerController> ().Health -= SpecialBulletDecreaser;
            if (other.GetComponent<PlayerController> ().Health <= 0) {
                Destroy (other.gameObject);
            }
        }
    }
    void DestroyBullet () {
        Destroy (this.gameObject);
    }

}