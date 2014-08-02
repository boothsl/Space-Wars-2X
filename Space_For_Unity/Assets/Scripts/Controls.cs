using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {
	public float y;
	public int width;
	public int height;
	public int maxLength;
	public GUIStyle backgroundGuiStyle;

	public GUIContent content;
	void OnGUI() {
		GUI.TextArea(new Rect(0, Screen.height/y, width, height), 
		             "\n2 Player Flight Controls:\n\n\n" +
		             "             - PLAYER 1 -     - PLAYER 2 -\n\n" +
		             "Move        W A S D       Arrow Keys\n\n\n" +
		             "Fire          Spacebar              Ctrl\n\n\n" +
		             "Thrust      Left Shift        Right Shift\n\n\n" +
		             "Brake       Left Shift        Right Shift\n\n\n",


		             maxLength, backgroundGuiStyle);
	}
}
