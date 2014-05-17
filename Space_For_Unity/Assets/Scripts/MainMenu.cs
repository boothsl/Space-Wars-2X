using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	

	public int button_width;
	public int button_height;

	public string targetScene;
	public GUIStyle startGuiStyle;
	//public GUIStyle menuOptionStyle;

	public GameObject instructionsP1;
	public GameObject instructions;
	public GameObject credits;



	void CreateButton(float x, float y, float width, float height, string text, GUIStyle the_style, int action) {
		float hw = width / 2;
		float hh = height / 2;
		if (GUI.Button(new Rect(x - hw, y - hh, width, height), text, the_style)) {
			switch(action) {
			case 0:
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
				break;
			case 2:
				if (instructions.activeInHierarchy) {
					instructions.SetActive(false);
				}
				else {
					instructions.SetActive(true);
				}

				instructionsP1.SetActive(false);
				credits.SetActive(false);
				break;
			case 3:
				if (credits.activeInHierarchy) {
					credits.SetActive(false);
				}
				else {
					credits.SetActive(true);
				}
				instructions.SetActive(false);
				instructionsP1.SetActive(false);
				break;
			}
		}

	}

	void OnGUI() {
		float width = Screen.width;
		float height = Screen.height;

		CreateButton (width / 4f, height * 0.7f, button_width, button_height, "1-Player", startGuiStyle, 0);
		CreateButton (width / 4f, height * 0.85f, button_width, button_height, "2-Player", startGuiStyle, 1);
		CreateButton (width * 3f / 4f, height * 0.7f, button_width, button_height, "How to Play", startGuiStyle, 2);
		CreateButton (width * 3f / 4f, height * 0.85f, button_width, button_height, "Credits", startGuiStyle, 3);
	}
}
