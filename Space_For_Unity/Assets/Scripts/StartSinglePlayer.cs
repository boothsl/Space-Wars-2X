using UnityEngine;
using System.Collections;

public class StartSinglePlayer : MonoBehaviour {

	public GameObject mainCamera;
	public GameObject instructionsP1;
	public GameObject gameTitle;

	//public Component thrustScript;

	// Update is called once per frame
	/*void Start () {
		thrustScript = Spaceship.GetComponent<SU_Thruster>();
		thrustScript.enabled = false;
	}*/

	void Update () {
		if (instructionsP1.activeInHierarchy && Input.GetKey("space")) {
			mainCamera.GetComponent<MainMenu>().enabled = false;
			instructionsP1.SetActive(false);
			gameTitle.SetActive(false);
		}

	}
}
