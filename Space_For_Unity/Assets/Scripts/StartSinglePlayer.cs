using UnityEngine;
using System.Collections;

public class StartSinglePlayer : MonoBehaviour {

	public GameObject mainCamera;
	public GameObject instructionsP1;
	public GameObject gameTitle;
	
	// Update is called once per frame
	void Update () {
		if (instructionsP1.activeInHierarchy && Input.GetKey("space")) {
			mainCamera.GetComponent<MainMenu>().enabled = false;
			instructionsP1.SetActive(false);
			gameTitle.SetActive(false);
		}
	}
}
