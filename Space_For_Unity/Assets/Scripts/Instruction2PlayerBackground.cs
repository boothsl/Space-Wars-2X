using UnityEngine;
using System.Collections;

public class Instruction2PlayerBackground : MonoBehaviour {
	public float y;
	public int width;
	public int height;
	public int maxLength;
	public GUIStyle backgroundGuiStyle;

	public GUIContent content;
	void OnGUI() {
		GUI.TextArea(new Rect(Screen.width/2 - width/2, Screen.height/y, width, height), 
		             "HOW TO PLAY\n\n" +
		             "            Player 1         Player 2\n" +
		             "Move        WASD           Arrow keys\n" +
		             "Thrust      Shift               Shift\n" +
		             "Fire        Spacebar            ?-key", maxLength, backgroundGuiStyle);
	}
}
