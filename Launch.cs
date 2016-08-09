using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Launch : MonoBehaviour {
	public Rigidbody2D rocketBody;
	public float rocketForce = 10.0f;
	public float throttleMutiple = 1.0f;
	public float maxRocketForce = 400.0f;

	public bool engineState = false;

	public Slider throttle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// update the throttle multiplier
		throttleMutiple = throttle.value;

		// update rocketForce based on throttle value
		rocketForce = maxRocketForce * throttleMutiple;

		if (engineState == true) {
			rocketBody.AddForce (transform.up * rocketForce);
		} else {
			rocketBody.AddForce (this.transform.up * 0.0f);
		}
	}

	public void luanchRocket() {
		if (engineState == false) {
			engineState = true;
		} else {
			engineState = false;
		}
	}
}
