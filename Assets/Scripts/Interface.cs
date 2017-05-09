using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour {

    public Canvas ui;
    private Text points;
    private Text finalScore;
    private Button restartButton;
    private Transform panelPoints;
    private Transform panelFinalScore;

    private Movement carObject;

	// Use this for initialization
	void Start () {
        points = ui.transform.Find("Points").GetComponent<Text>();
        finalScore = ui.transform.Find("FinalScore").GetComponent<Text>();
        panelPoints = ui.transform.Find("PointsBackground");
        panelFinalScore = ui.transform.Find("FinalScoreBackground");
        restartButton = ui.transform.Find("RestartButton").GetComponent<Button>();

        panelFinalScore.gameObject.SetActive(false);

        restartButton.transform.gameObject.SetActive(false);

        carObject = this.GetComponent<ObjectsGeneration>().GetCar().GetComponent<Movement>();
    }
	
	// Update is called once per frame
	void Update () {
        if (carObject.HasHit()) {
            points.enabled = false;
            panelPoints.gameObject.SetActive(false);

            panelFinalScore.gameObject.SetActive(true);
            restartButton.transform.gameObject.SetActive(true);
            finalScore.text = "Final Score\n" + Mathf.Round(carObject.GetScore());
        }
        else {
            points.text = "Speed: " + Mathf.Round(carObject.GetSpeed());
        } 
	}

    public void RestartScene() {
        SceneManager.LoadScene("Game");
    }
}
