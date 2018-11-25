using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossMover : MonoBehaviour {
    Vector2 To;
    public float moveDistance = 3f;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start () {
        To = GameObject.FindGameObjectWithTag ("BosPosition").transform.position;
        transform.position = Vector2.MoveTowards (transform.position, To, moveDistance * Time.deltaTime);
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update () {
        transform.position = Vector2.MoveTowards (transform.position, To, moveDistance * Time.deltaTime);
        if (transform.position.x == To.x && transform.position.y == To.y) {
            transform.position = transform.position;
        }
    }

}