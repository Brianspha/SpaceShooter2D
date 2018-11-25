using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 5f;
    Vector2 moveVeloc;
    public float Health = 10f;
    public int Score { get; set; }
    public float MaxY = 4.39f;
    public float MinY = -4.39f;
    public float MaxX = -28.99f;
    Rigidbody2D player;
    // Use this for initialization
    void Start () {
        player = GetComponent<Rigidbody2D> ();
    }

    // Update is called once per frame
    void Update () {
        //Get input from user using GetRawAxis allows us to not specify the exact buttons that will be used 
        //for up and down and allows the user the freedom to use anybutton that is used for moving foward up down etc
        Vector2 move = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
        moveVeloc = move.normalized * speed; //multiply the position with which the user wants to move to
    }
    void FixedUpdate () {
        if ((player.position + moveVeloc * Time.deltaTime).y <= MaxY && (player.position + moveVeloc * Time.deltaTime).y >= MinY && (player.position + moveVeloc * Time.deltaTime).x >= MaxX) {
            player.MovePosition (player.position + moveVeloc * Time.deltaTime); //use the postion the user wishes to move and add it to the current position of the user
        }
    }
}