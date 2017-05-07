using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    private float curSpeed = 2F;
    private float acceleration = 4;
    private float score = 0F;

    private float MAX_X = 1.8F;
    private float MIN_X = -1.8F;

    private bool hasHit = false;
	
	// Update is called once per frame
	void Update () {
        GetMovement();
        SetSpeed();
	}

    void GetMovement(){
        if (SystemInfo.supportsAccelerometer && !hasHit){
            if (Input.acceleration.x > 0 && this.transform.position.x < MAX_X){
                this.transform.Translate(Vector3.right * Input.acceleration.x);
            }
            else if (Input.acceleration.x < 0 && this.transform.position.x > MIN_X) {
                this.transform.Translate(Vector3.left * (-Input.acceleration.x));
            }
        }
    }

    void SetSpeed(){
        this.transform.Translate(Vector3.forward * curSpeed * Time.deltaTime);
        curSpeed += acceleration * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision) {
        if (!hasHit && collision.gameObject.name == "Barrier") {
            score = curSpeed;
            acceleration = 0F;
            curSpeed = 2.0F;
            hasHit = true;
        }
    }

    public float GetSpeed() {
        return curSpeed;
    }

    public float GetScore() {
        return score;
    }

    public bool HasHit() {
        return hasHit;
    }
}
