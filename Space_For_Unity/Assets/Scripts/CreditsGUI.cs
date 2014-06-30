using UnityEngine;
using System.Collections;

public class CreditsGUI : MonoBehaviour {

	public float y;
	public int width;
	public int height;
	public int maxLength;
	public GUIStyle backgroundGuiStyle;
	
	public GUIContent content;
	void OnGUI() {
		GUI.TextArea(new Rect(Screen.width/2 - width/2, Screen.height/y, width, height), 
		             "\nCredits:\n\n" +
		             "Howard DeCastro\n" +
		             "Chris Hull\n" +
		             "Stephanie Booth\n" +
		             "Akhilesh Patel\n" +
		             "Martin Smith\n" +

	//	             maxLength, backgroundGuiStyle);

		             "John Thomas\n" +
		             "Alan McCosh\n" +
		             "Tyler Gregg\n" +
		             "Marcellus Wilson\n" +
		             "Alexander Reinhard",

		             maxLength, backgroundGuiStyle);
	}
}
