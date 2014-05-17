using UnityEngine;
using System.Collections;

public class Instructions1Player : MonoBehaviour {

	public float y;
	public int width;
	public int height;
	public int maxLength;
	public GUIStyle backgroundGuiStyle;
	
	public GUIContent content;
	void OnGUI() {
		GUI.TextArea(new Rect(Screen.width/2 - width/2, Screen.height/y, width, height), 
		             "Instructions\n\n" +
		             "Move        WASD\n" +
		             "Thrust        Shift\n" +
		             "Fire        Spacebar\n\n" +
		             "Press Space to Begin", maxLength, backgroundGuiStyle);
	}
}
