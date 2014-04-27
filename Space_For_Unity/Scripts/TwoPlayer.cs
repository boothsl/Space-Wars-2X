using UnityEngine;
using System.Collections;

public class TwoPlayer : MonoBehaviour {
	

	public int button_width;
	public int button_height;

	public string targetScene;
	public GUIStyle startGuiStyle;

	public GameObject instructions;
	public GameObject credits;
	public GameObject aboutUs;

	void CreateButton(float x, float y, float width, float height, string text, GUIStyle the_style, int action) {
		float hw = width / 2;
		float hh = height / 2;
		if (GUI.Button(new Rect(x - hw, y - hh, width, height), text, the_style)) {
			switch(action) {
			case 0:
				Application.LoadLevel(targetScene);
				break;
			case 1:
				aboutUs.SetActive(true);
				instructions.SetActive(false);
				credits.SetActive(false);
				break;
			case 2:
				instructions.SetActive(true);
				credits.SetActive(false);
				aboutUs.SetActive(false);
				break;
			case 3:
				credits.SetActive(true);
				instructions.SetActive(false);
				aboutUs.SetActive(false);
				break;
			}
		}

	}


	void OnGUI() {
		float width = Screen.width;
		float height = Screen.height;

		CreateButton (width / 4f, height * 0.7f, button_width, button_height, "2-Player", startGuiStyle, 0);
		CreateButton (width / 4f, height * 0.85f, button_width, button_height, "About Us", startGuiStyle, 1);
		CreateButton (width * 3f / 4f, height * 0.7f, button_width, button_height, "How to Play", startGuiStyle, 2);
		CreateButton (width * 3f / 4f, height * 0.85f, button_width, button_height, "Credits", startGuiStyle, 3);

	}
}
