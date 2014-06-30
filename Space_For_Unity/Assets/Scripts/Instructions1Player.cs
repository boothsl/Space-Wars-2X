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

		             "\nFlight Controls:\n\n" +
		             "\nMove . . . . . Mouse\n\n" +
		             "Fire . . . . . . . . RMB\n\n" +
		             "Thrust . . . . . . Return\n\n" +
		             "Brake . . . . Right Shift\n\n\n" +

		             "** Press Space to Begin **", maxLength, backgroundGuiStyle);
	}
}
