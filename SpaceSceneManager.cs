using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpaceSceneManager : MonoBehaviour {

	public float conversionFactor = 57.2958f;
	public Vector3 lookAtPoint;

	public Text moonDistText;
	public Text partVelText;

	public Rigidbody2D thirdStage;
	public Rigidbody2D planetBody;

	public Rigidbody2D fairingLeft;
	public Rigidbody2D fairingRight;

	public Rigidbody2D serviceModule;

	public Rigidbody2D LEM;

	public Rigidbody2D serviceModDockingPort;

	public FixedJoint2D fairingLeftJoint;
	public FixedJoint2D fairingRightJoint;
	public FixedJoint2D thirdStageJoing;
	public FixedJoint2D serviceModJoint;

	public GameObject fairingLeftObject;
	public GameObject fairingRightObject;

	public GameObject moonObject;
	public Rigidbody2D moonBody;

	public GameObject moonPointer;

	public float moonDistY;
	public float moonDistX;
	public float moonDist;
	public float moonAngle;

	public GameObject lem;

	// Track which thruster buttons have been activated
	public bool rightThrust = false;
	public bool leftThrust = false;
	public bool forwardThrust = false;
	public bool aftThrust = false;
	public bool rightRotate = false;
	public bool leftRotate = false;

	// Array for all space parts in a scene
	public GameObject[] allParts;

	public int orbitalVeclocity;

	public PolygonCollider2D lemCollider;

	public Docking dockingScript;

	// Use this for initialization
	void Start () {
		// Set initial value for orbitalVelocity
		orbitalVeclocity = 1;
		dockingScript = serviceModDockingPort.GetComponent<Docking> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Update array of space parts in the scene
		allParts = GameObject.FindGameObjectsWithTag ("SpacePart");

		partManuever ();
		MoonDistCalc ();

		//print ("Orbital velocity: " + orbitalVeclocity);
	}

	public void changeScene() {
		SceneManager.LoadScene ("LaunchScene");
	}

	// Separate the fairing
	public void fairingSep() {
		// Destroy all fairing joints
		Destroy(fairingLeftJoint);
		Destroy (fairingRightJoint);
		Destroy (thirdStageJoing);
		Destroy (serviceModJoint);

		fairingLeft.AddForce (fairingLeft.transform.right * -30);
	    fairingRight.AddForce (fairingRight.transform.right * 30);

		StartCoroutine (waitForABit());
	}

	public void engines() {
		++orbitalVeclocity;
	}

	// Add force the leave vicinity of planet once orbit is broken
	public void leavingOrbit() {
	}

	// Wait for a little bit before destroying the fairing after fairing sep
	IEnumerator waitForABit() {
		yield return new WaitForSeconds (2);

		// destroy the fairing
		Destroy(fairingLeft);
		Destroy (fairingRight);

		Destroy (fairingLeftObject);
		Destroy (fairingRightObject);

		addCollider ();
	}

	// Add a collider to the lem
	public void addCollider () {
		lemCollider = lem.AddComponent<PolygonCollider2D> ();
	}

	// Turn on the right thruster
	public void rightThrusterOn () {
		rightThrust = true;
	}

	// Turn off the right thruster
	public void rightThrusterOff() {
		rightThrust = false;
	}

	// Turn on the left thruster
	public void leftThrusterOn() {
		leftThrust = true;
	}

	// Turn off the left thruster
	public void leftThrustOff() {
		leftThrust = false;
	}

	// Turn on the forward thruster
	public void forwardThrusterOn() {
		forwardThrust = true;
	}

	// Turn off the forward thruster
	public void forwardThrusterOff() {
		forwardThrust = false;
	}

	// Turn on the aft thruster
	public void aftThrusterOn() {
		aftThrust = true;
	}

	// Turn off the aft thruster
	public void aftThrusterOff() {
		aftThrust = false;
	}

	// Turn on right rotation thrust
	public void rightRotateOn() {
		rightRotate = true;
	}

	// Turn off right rotatation thrust
	public void rightRotateOff() {
		rightRotate = false;
	}

	// Turn on left rotation thrust
	public void leftRotateOn () {
		leftRotate = true;
		print ("Left rotate on");
	}

	// Turn off left rotation thrust
	public void leftRotationOff() {
		leftRotate = false;
	}

	// Manuver the active space part
	public void partManuever () {
		foreach (GameObject part in allParts) {
			// Verfiy whether or not a part is active and if it has thrusters
			if (part.GetComponent<SpacePart> ().stageIsActive == true && part.GetComponent<SpacePart>().hasThrusters == true) {
				if (rightThrust == true) {
					part.GetComponent<SpacePart> ().partBody.AddForce (part.GetComponent<SpacePart> ().transform.right * 3.0f);
				} else if (leftThrust == true) {
					part.GetComponent<SpacePart> ().partBody.AddForce (part.GetComponent<SpacePart> ().transform.right * -3.0f);
				} else if (forwardThrust == true) {
					part.GetComponent<SpacePart> ().partBody.AddForce (part.GetComponent<SpacePart> ().transform.up * 3.0f);
				} else if (aftThrust == true) {
					part.GetComponent<SpacePart> ().partBody.AddForce (part.GetComponent<SpacePart> ().transform.up * -3.0f);
				} else if (rightRotate == true) {
					part.GetComponent<SpacePart> ().partBody.rotation -= 2.0f;
				} else if (leftRotate == true) {
					part.GetComponent<SpacePart> ().partBody.rotation += 2.0f;
				}
			}
		}
	}

	// Seperate the third stage
	public void thirdStageSep() {
		print (dockingScript.docked);

		if (dockingScript.docked == true) {
			Destroy (LEM.GetComponent<FixedJoint2D> ());
			thirdStage.AddForce (thirdStage.transform.up * -3.0f);
		}
	}

	// Calculate the distance from the rocket to the moon
	public void MoonDistCalc() {
		foreach (GameObject currentPart in allParts) {
			if (currentPart.GetComponent<SpacePart> ().stageIsActive == true) {

				moonDistX = currentPart.GetComponent<SpacePart> ().partBody.position.x - moonBody.position.x;
				moonDistY = currentPart.GetComponent<SpacePart> ().partBody.position.y - moonBody.position.y;
				moonDist = Mathf.Sqrt ((moonDistX * moonDistX) + (moonDistY * moonDistY));

				moonDistText.text = moonDist.ToString ();
				partVelText.text = currentPart.GetComponent<SpacePart> ().partBody.velocity.ToString ();

				moonAngle = Mathf.Tan (moonDistY / moonDistX) * Mathf.Rad2Deg;

				Vector3 moonPos = moonObject.transform.position;

				moonPointer.transform.rotation = Quaternion.LookRotation (Vector3.forward, moonPos - moonPointer.transform.position);

				//print ("Dist X: " + moonDistX);
				//print ("Dist Y: " + moonDistY);
				//print("Distance to moon: " + moonDist);
				//print ("Angle to moon: " + moonAngle);
				//print ("Stage rotation: " + currentPart.GetComponent<SpacePart> ().partBody.rotation);
			}
		}
	}
}
