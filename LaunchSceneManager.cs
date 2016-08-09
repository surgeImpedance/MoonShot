using UnityEngine;
using System.Collections;
using ProgressBar;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LaunchSceneManager : MonoBehaviour {

	public RocketPart currentActivePart;

	// keep track of all the rocket parts in the scene
	public RocketPart[] parts; 

	public ProgressBarBehaviour fuelLevelBar;
	public ProgressBarBehaviour loxLevelBar;

	// Angle for rocket turning
	public float turnAngle = 0.0f;

	// Angle in radians
	public float angleRadians = 0.0f;

	// Angle conversion constant
	public float angleConstant = 180.0f / 3.14159f;

	// Variables for camera follow
	public Vector3 myPos;
	//public Transform myPlay;
	public Camera myCam;

	// References for labels
	public Text latVelLabel;
	public Text vertVelLabel;
	public Text altLabel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		updateFuelLevel ();
		updateLOXLevel ();

		updateLabels ();

		separateTheStage ();

		//updateCameraFollow ();

		shouldShowExhaust ();
	}

	void FixedUpdate () {
		applyThrust ();
	}

	// Changes the active stage, based on fuel level
	public void changeActiveStage() {
	}

	// Update the fuel bar level
	public void updateFuelLevel() {
		// Cycle through all of the rocket parts in the scene
		foreach (RocketPart rPart in parts) {
			// Upate the fuel level and the level bar based on if the stage is active, if the stage has a fuel tank, and if the engine is on or off
			if (rPart.activeStage == true && rPart.hasFuelTank == true) {
				// Update the fuel level bar
				fuelLevelBar.Value = (rPart.fuelLevel / rPart.fuelCapacity) * 100;

				// If the engine is on, then subtract fuel, but only if the fuel amount is greater than zero
				if (rPart.engineState == true) {
					if (rPart.fuelLevel > 0) {
						rPart.fuelLevel -= 1.0f;
					}
				}
			}
		}
	}

	// Update the lox bar level
	public void updateLOXLevel() {
		// Cycle through all of the rocket parts in the scene
		foreach (RocketPart rPart in parts) {
			// Update the lox level and the level bar based if the stage is active, if the stage has a fuel tank, and if the engine is on or off
			if (rPart.activeStage == true && rPart.hasFuelTank == true) {
				// Update the lox level bar
				loxLevelBar.Value = (rPart.loxLevel / rPart.loxCapacity) * 100;

				// If the engine is on, then subtract fuel, but only if the fuel amount is greater than zero
				if (rPart.engineState == true) {
					if (rPart.loxLevel > 0) {
						rPart.loxLevel -= 1.0f;
					}
				}
			}
		}
	}

	public void toggleActiveStageEngineStatus () {
		// Cycle through all of the rocket parts in the scene, and toggle the engine status of the currently active part
		foreach (RocketPart rocketPart in parts) {
			if (rocketPart.activeStage == true && rocketPart.hasFuelTank == true) {
				rocketPart.engineState = !rocketPart.engineState;
			}
		}
	}

	// Turn the currently active stage left
	public void turnActiveLeft () {
		// Loop through all parts to find the active stage
		foreach (RocketPart rocketPart in parts) {
			if (rocketPart.activeStage == true) {
				//rocketPart.stageBody.rotation += 1.0f;
				//rocketPart.stageBody.AddTorque(1.0f);
				turnAngle += 1.0f;
			}
		}
	}

	// Convert angle in degrees to angle in radians
	public void convertAngle() {
		angleRadians = turnAngle / angleConstant;
	}

	// Rotate the rocket to the left
	public void rotateLeft() {
		foreach (RocketPart rocketPart in parts) {
			rocketPart.stageBody.rotation += 1.0f;
		}
	}

	// Rotate the rocket to the right
	public void rotateRight() {
		foreach (RocketPart rocketPart in parts) {
			rocketPart.stageBody.rotation -= 1.0f;
		}
	}

	// Apply thrust to the rocket
	public void applyThrust() {
		// Apply thrust to the active stage
		foreach (RocketPart rPart in parts) {
			if (rPart.activeStage == true && rPart.engineState == true && rPart.fuelLevel > 0) {
				// Update the throttle multiplier
				rPart.throttleMutiple = rPart.throttle.value;

				// Update rocketForce based on throttle value
				rPart.rocketForce = rPart.maxRocketForce * rPart.throttleMutiple;

				rPart.stageBody.AddForce (rPart.stageBody.transform.up * rPart.rocketForce);
			} 
		}
	}
		
	// Update the altitude and velocity labels
	public void updateLabels() {
		foreach (RocketPart rocketPart in parts) {
			if (rocketPart.activeStage == true) {
				latVelLabel.text = rocketPart.stageBody.velocity.x.ToString ("F1");
				vertVelLabel.text = rocketPart.stageBody.velocity.y.ToString ("F1");
				altLabel.text = rocketPart.stageBody.position.y.ToString ("F1");
			}
		}
	}

	// Stage sep
	public void separateTheStage() {
		foreach (RocketPart rocketPart in parts) {
			// next index
			int nextIndex;

			if (rocketPart.activeStage == true && rocketPart.stageSeparated == true) {
				rocketPart.GetComponent<FixedJoint2D> ().enabled = false;
				rocketPart.activeStage = false;
				rocketPart.engineState = false;

				//rocketPart.stageBody.AddForce (rocketPart.stageBody.transform.up * -10000.0f);

				nextIndex = System.Array.IndexOf (parts, rocketPart) + 1;
				if (parts [nextIndex] != null) {
					parts [nextIndex].activeStage = true;
				}
			}
		}
	}

	public void setStageSepStatus() {
		foreach(RocketPart rPart in parts) {
			if (rPart.activeStage == true) {
				rPart.stageSeparated = true;
			}
		}
	}

	// Determine whether or not to show exhaust
	public void shouldShowExhaust() {
		foreach (RocketPart rPart in parts) {
			if (rPart.engineState == true) {

				// only turn on exhaust for the first stage
				if (rPart.tag == "first_stage") {
					// turn on the exhaust
					rPart.firstStageExhaust_1.Play ();
					rPart.firstStageExhaust_2.Play ();
					rPart.firstStageExhaust_3.Play ();
				} else if (rPart.tag == "second_stage") {
					rPart.secondStageExhaust_1.Play ();
					rPart.secondStageExhaust_2.Play ();
					rPart.secondStageExhaust_3.Play ();
				} else if (rPart.tag == "third_stage") {
					rPart.thirdStageExhaust.Play ();
				}
			} else {
				//only turn off the exhaust for the first stage
				if (rPart.tag == "first_stage") {
					rPart.firstStageExhaust_1.Stop ();
					rPart.firstStageExhaust_2.Stop ();
					rPart.firstStageExhaust_3.Stop ();
				} else if (rPart.tag == "second_stage") {
					rPart.secondStageExhaust_1.Stop ();
					rPart.secondStageExhaust_2.Stop ();
					rPart.secondStageExhaust_3.Stop ();
				} else if (rPart.tag == "third_stage") {
					rPart.thirdStageExhaust.Stop ();
				}
			}
		}
	}

	public void changeScenes() {
		SceneManager.LoadScene ("SpaceScene");
	}
}
