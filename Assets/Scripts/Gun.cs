using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    public List<GameObject> Bullets;
    public GameObject bullet;
    private Transform enemy;
    public string WhichPlayer;
    public float MaxTimeBetweenShots = .002f;
    public float CurrentTimeSinceLastShot = 0;
    public GameObject RocketObj;
    public int MaxRocket = 10;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start () {
        //   enemy= GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update () {
        if (Input.GetKey (KeyCode.Space) && CurrentTimeSinceLastShot <= 0) {
            Shoot ();
            CurrentTimeSinceLastShot = MaxTimeBetweenShots;
        }
        if (Input.GetKey (KeyCode.R) && CurrentTimeSinceLastShot <= 0 && MaxRocket > 0) {
            ShootRocket ();
            CurrentTimeSinceLastShot = MaxTimeBetweenShots;
            MaxRocket--;
            GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().shakeAmount = 2f;
            GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().shakeDuration = 1f;
            GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShake> ().ShakeCamera = true;

        }
        if (CurrentTimeSinceLastShot <= 0) {
            CurrentTimeSinceLastShot = MaxTimeBetweenShots;
        }
        CurrentTimeSinceLastShot -= Time.deltaTime;
    }
    /**
     *@dev invoked when user presses the space key
     */
    void Shoot () {
        Instantiate (bullet, transform.position, Quaternion.identity);
    }

    void ShootRocket () {
        Instantiate (RocketObj, transform.position, Quaternion.identity);

    }

}