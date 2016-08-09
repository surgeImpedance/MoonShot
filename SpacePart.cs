using UnityEngine;
using System.Collections;

public class SpacePart : MonoBehaviour {

	// Reference to this particular game object
	public GameObject partGameOb;

	// Reference to this object's rigidbody
	public Rigidbody2D partBody;


	// Status of the stage, active or not
	public bool stageIsActive = false;

	// Does this part have thrusters?
	public bool hasThrusters = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnMouseDown() {
		StartCoroutine (FlashRed ());

		ToggleActive ();
	}

	// Flash stage to indicate that this particular stage was selected
	IEnumerator FlashRed() {
		partGameOb.GetComponent<Renderer> ().material.color = new Color (240.0f / 255.0f, 128.0f/255.0f, 128.0f/255.0f, 1.0f);
		yield return new WaitForSeconds (0.25f);
		partGameOb.GetComponent<Renderer> ().material.color = Color.white;

		yield return new WaitForSeconds (0.25f);
		partGameOb.GetComponent<Renderer> ().material.color = new Color (240.0f / 255.0f, 128.0f/255.0f, 128.0f/255.0f, 1.0f);

		yield return new WaitForSeconds (0.25f);
		partGameOb.GetComponent<Renderer> ().material.color = Color.white;
	}

	// Toggle between being the active stage and not active stage
	public void ToggleActive() {
		stageIsActive = !stageIsActive;
	}
}
