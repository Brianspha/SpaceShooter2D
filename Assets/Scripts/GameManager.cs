using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {
    public float Score { get; set; }
    public Text ScoreText;
    public Text HealthScore;

    private void Start () {
        ScoreText.text = "--";
    }
    private void Update () {
        HealthScore.text = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().Health.ToString ();
        ScoreText.text = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().Score.ToString ();

    }

}