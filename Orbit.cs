using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {
	public Rigidbody2D part;
	public Rigidbody2D planet;

	// Moon
	public Rigidbody2D moon;

	// Access the space scene manager
	public SpaceSceneManager sceneMan;

	// Access the space scene manager script
	public SpaceSceneManager spaceSceneScript;

	// Determine if the rocket part is in orbit or not
	public bool isInOrbit = true;

	// Calculate x and y components of velocity
	public float xComponent;
	public float yComponent;

	public float distanceY;
	public float distanceX;

	public float earthCoordY;
	public float earthCoordX;

	public int velocity;

	// Distance from selected part to the moon
	public float distanceToMoon;

	// Use this for initialization
	void Start () {
		velocity = 1;

		// Set up the space scene manager script
		spaceSceneScript = sceneMan.GetComponent<SpaceSceneManager>();
	}

	// Update is called once per frame
	void Update () {

		// Update the orbital velocity
		velocity = sceneMan.orbitalVeclocity;
	
		if (isInOrbit == true) {
			if (part != null) {
				part.transform.RotateAround (planet.transform.position, Vector3.forward, -velocity);
			}
		}

		breakOrbit ();

		// Update the distance to the moon
		distanceToMoon = spaceSceneScript.moonDist;

		// Check to see if we are close enough for lunar orbit
		InLunarOrbit();
	}

	// Decide whether or not to break orbit
	public void breakOrbit() {
		if (velocity >= 5) {
			isInOrbit = false;

			if (Mathf.Abs (distanceX) >= 3 && Mathf.Abs (distanceY) >= 3) {
				velocity = 0;
				spaceSceneScript.orbitalVeclocity = 1;
				print (spaceSceneScript.orbitalVeclocity);
			}

			//part.velocity = part.transform.up * velocity;

			part.AddForce (part.transform.up * velocity);

			earthCoordX = planet.position.x;
			earthCoordY = planet.position.y;
			distanceX = part.position.x - earthCoordX;
			distanceY = part.position.y - earthCoordY;

			/*print ("X: " + distanceX);
			print ("Y: " + distanceY);
			print ("Velocity = " + velocity);*/
		}
	}

	// Decide if we need to orbit the moon
	public void InLunarOrbit() {
		//print (distanceToMoon);

		if (distanceToMoon <= 4 && isInOrbit == false && part.isKinematic == false) {
			planet = moon;
			isInOrbit = true;
			//print("Distance less than 5");
			//print(velocity);
		}
	}
		
}
