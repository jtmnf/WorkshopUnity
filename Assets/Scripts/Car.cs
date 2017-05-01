using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour {

	public TerrainGeneration terrain;
	public UserInterface ui;

	private float MAX_LEFT_CAR_X = 8.33F;
	private float MAX_RIGHT_CAR_X = 4.13F;


	public float acceleration = 1.0F;
	private float curSpeed = 0F;

	// Update is called once per frame
	void Update () {
		if (acceleration > 0F) {
			transform.Translate (Vector3.forward * curSpeed * Time.deltaTime);
			curSpeed += acceleration * Time.deltaTime;

			ui.GetValue().text = "Kilometers:\n" + Mathf.Floor(curSpeed * 3.6F).ToString ();

			getTouchs ();
			terrain.calculatePositionsOfObjects ();
		}
	}

	IEnumerator OnTriggerEnter(Collider col){
		float maxSpeed = curSpeed*3.6F;
		curSpeed = 0F;
		acceleration = 0F;

		ui.GetValue().text = "";

		ui.GetImage ().enabled = true;
		ui.GetFinalScore().text = "Final Score: " + Mathf.Floor(maxSpeed).ToString ();

		yield return new WaitForSeconds (3);
		SceneManager.LoadScene ("1");
	}

	private void getTouchs(){
		if (SystemInfo.supportsAccelerometer) {
			if ((-Input.acceleration.x) > 0 && transform.position.x < MAX_LEFT_CAR_X) {
				transform.Translate (Vector3.left * (-Input.acceleration.x));
			}
			if ((-Input.acceleration.x) < 0 && transform.position.x > MAX_RIGHT_CAR_X) {
				transform.Translate (Vector3.left * (-Input.acceleration.x));
			}
		}
		else{
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Stationary) {
				Vector2 fingerPos = Input.GetTouch (0).position;

				float widthAtHalf = Screen.width / 2;

				if ((widthAtHalf - fingerPos.x) > 0 && transform.position.x < MAX_LEFT_CAR_X) {
					transform.Translate (Vector3.left * 0.1F);
				}

				if ((widthAtHalf - fingerPos.x) < 0 && transform.position.x > MAX_RIGHT_CAR_X) {
					transform.Translate (Vector3.left * (-0.1F));
				}
			}
		}
	}		
}

