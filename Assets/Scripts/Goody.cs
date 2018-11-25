using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goody : MonoBehaviour {

    public Transform Player;
    public Vector2 target;
    public float Speed = 20f;
    void Start () {
        Player = GameObject.FindGameObjectWithTag ("Player").transform;
        target = new Vector2 (Player.position.x, this.transform.position.y);

    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update () {
        transform.position = Vector2.MoveTowards (transform.position, target, Time.deltaTime * Speed);
    }

}