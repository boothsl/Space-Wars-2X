using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	public string winner = "no one";

	public GUIStyle GameOverStyle;

	void OnGUI()
	{
		int width = Screen.width;
		int height = Screen.height;

		GUI.TextArea( 
		     new Rect( width * 0.15f, height * 0.35f, width * 0.7f, height * 0.1f ),
		     "Game over! " + winner + " is the winner!",
		     GameOverStyle); 

	}
}
