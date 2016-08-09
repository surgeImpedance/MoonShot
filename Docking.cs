using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Docking : MonoBehaviour {

	// Is the port docked?
	public bool docked = false;

	// Reference to the LEM game object
	public GameObject LEM;

	// Reference to LEM rigidbody
	public Rigidbody2D LEM_Body;

	//public Button thirdStageSepBtn;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		CreateDocking ();
	}

	void OnCollisionEnter2D(Collision2D col) {
		//print ("Detected collision between " + gameObject.name + " and " + col.collider.name);

		if (col.gameObject.tag == "LEMDock") {
			docked = true;
		}
	}

	public void CreateDocking() {
		if (docked == true) {
			LEM.transform.parent = gameObject.transform;
			LEM.transform.GetComponent<Rigidbody2D> ().isKinematic = true;
		}
	}
}
