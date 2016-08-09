using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ProgressBar;

public class RocketPart : MonoBehaviour {
	public float fuelLevel;
	public float loxLevel;
	public float altitude;
	public float verticalVelocity;
	public float lateralVelocity;

	// Store mouse clicks
	int numClicks = 0;

	public float rocketForce = 10.0f;
	public float throttleMutiple = 1.0f;
	public float maxRocketForce = 400.0f;

	public Slider throttle;

	// capacity of the part's fuel tank
	public float fuelCapacity;

	// capacity of the part's lox tank
	public float loxCapacity;

	public bool activeStage;
	public bool engineState = false;
	public bool isFairing;

	public ParticleSystem firstStageExhaust_1;
	public ParticleSystem firstStageExhaust_2;
	public ParticleSystem firstStageExhaust_3;

	public ParticleSystem secondStageExhaust_1;
	public ParticleSystem secondStageExhaust_2;
	public ParticleSystem secondStageExhaust_3;

	public ParticleSystem thirdStageExhaust;

	// Track the part's stage separation status
	public bool stageSeparated = false;

	// Is this a part or a stage?
	public bool hasFuelTank = true;

	// Store a reference to the this stage
	public Rigidbody2D stageBody;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//updateFuelLevel ();
		//updateLoxLevel ();
		updateAltAndVel ();
		//updateLabels ();

		//Debug.Log ("Engine state: " + engineState);
		//Debug.Log ("Active stage: " + activeStage);

		//print (fuelLevel);

		//engineState = lScript.engineState;

		//flyStage ();
	}

	// Update the stage's fuel level if the stage is active, and if the engine is on
	public void updateFuelLevel () {
		if (engineState && activeStage) {
			fuelLevel -= 2.0f;
		}
	}

	// Update the stage's lox level if the stage is active, and if the engine is on
	public void updateLoxLevel () {
		if (engineState && activeStage) {
			loxLevel -= 2.0f;
		}
	}

	// Toggle the state of this stage
	public void toggleActive () {
		activeStage = !activeStage;
	}
		
	// Update stage's altitude and velocity
	public void updateAltAndVel () {
		verticalVelocity = stageBody.velocity.y;
		lateralVelocity = stageBody.velocity.x;
		altitude = stageBody.position.y;
	}
		
	// Fly the rocket
	public void flyStage() {
		if (engineState == true && activeStage == true) {
			// update the throttle multiplier
			throttleMutiple = throttle.value;

			// update rocketForce based on throttle value
			rocketForce = maxRocketForce * throttleMutiple;

			if (engineState == true && fuelLevel > 0) {
				stageBody.AddForce (transform.up * rocketForce);
			} else {
				stageBody.AddForce (transform.up * 0.0f);
			}
		}
	}
		
	/*public void OnMouseDown() {
		++numClicks;

		// If player double clicks, then separate stage
		if (numClicks == 2) {
			activeStage = !activeStage;
			numClicks = 0;
		}  
	}

	// Stage sep
	public void OnMouseDrag() {
		this.GetComponent<FixedJoint2D> ().enabled = false;
		this.engineState = false;
		this.activeStage = false;
	}*/
}