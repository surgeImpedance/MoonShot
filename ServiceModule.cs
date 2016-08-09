using UnityEngine;
using System.Collections;

public class ServiceModule : MonoBehaviour {

	// Reference to the service module game object
	public GameObject partGameOb;

	// Status of the stage, active or not
	public bool stageIsActive = false;

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
