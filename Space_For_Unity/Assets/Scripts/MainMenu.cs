using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	

	public int button_width;
	public int button_height;

	public string targetScene;
	public GUIStyle startGuiStyle;
	//public GUIStyle menuOptionStyle;

	//public GameObject instructionsP1;
	public GameObject controls;
	public GameObject credits;
	public GameObject startServerGUI;
	public GameObject startClientGUI;


	void CreateButton(float x, float y, float width, float height, string text, GUIStyle the_style, int action) {
		float hw = width / 2;
		float hh = height / 2;
		if (GUI.Button(new Rect(x - hw, y - hh, width, height), text, the_style)) {
			switch(action) {
			/*case 0:
				if (instructionsP1.activeInHierarchy){
					instructionsP1.SetActive(false);
				}
				else {
					instructionsP1.SetActive(true);
				}
				instructions.SetActive(false);
				credits.SetActive(false);
				break;
			case 1:
				Application.LoadLevel(targetScene);
				break;*/
			case 0:
				startServerGUI.SetActive(true);
				this.enabled = false;
				break;
			case 1:
				startClientGUI.SetActive(true);
				this.enabled = false;
				break;
			case 2:
				if (controls.activeInHierarchy) {
					controls.SetActive(false);
				}
				else {
					controls.SetActive(true);
				}

				//instructionsP1.SetActive(false);
				credits.SetActive(false);
				break;
			case 3:
				if (credits.activeInHierarchy) {
					credits.SetActive(false);
				}
				else {
					credits.SetActive(true);
				}
				controls.SetActive(false);
				//instructionsP1.SetActive(false);
				break;

			}

		}

	}

	void OnGUI() {
		float width = Screen.width;
		float height = Screen.height;

		//CreateButton (width / 4f, height * 0.65f, button_width, button_height, "1-Player", startGuiStyle, 0);
		//CreateButton (width / 4f, height * 0.80f, button_width, button_height, "2-Player", startGuiStyle, 1);
		CreateButton (width / 2f, height * 0.45f, button_width, button_height, "Single Player", startGuiStyle, 0);
		CreateButton (width / 2f, height * 0.60f, button_width, button_height, "Multiplayer", startGuiStyle, 1);
		CreateButton (width * 7f / 8f, height * 0.75f, button_width, button_height, "Controls", startGuiStyle, 2);
		CreateButton (width * 7f / 8f, height * 0.90f, button_width, button_height, "Credits", startGuiStyle, 3);

	}
}
