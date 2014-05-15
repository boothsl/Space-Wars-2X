using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {
	public Camera myCamera;
	public GUIStyle borderGUIStyle;

	void OnGUI() {
		float width = myCamera.pixelWidth;
		float height = myCamera.pixelHeight;

		GUI.Box (new Rect (0, 0, width, height), "", borderGUIStyle);
	}
}
