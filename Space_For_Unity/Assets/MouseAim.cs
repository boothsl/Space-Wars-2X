using UnityEngine;
using System.Collections;

public class MouseAim : MonoBehaviour {
	[SerializeField] Transform mouseAimObject;
	[SerializeField] Texture2D crosshairTexture;

	private GUITexture crosshair;
	// Use this for initialization
	void Start () {
		Screen.showCursor = false;
		crosshair.texture = crosshairTexture;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mousePos = Input.mousePosition;
		Vector3 wantedPos = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, 1000.0f));
		transform.position = wantedPos;
		transform.parent.LookAt(wantedPos);
	}

	
		
	void OnGUI()	
	{
		Vector3 mousePos = Event.current.mousePosition;
		
		mousePos.x = Mathf.Clamp(mousePos.x, Screen.width/Screen.width, Screen.width);
		mousePos.y = Mathf.Clamp(mousePos.y, Screen.height/Screen.height, Screen.height);

		

		GUI.DrawTexture(new Rect( mousePos.x - ((Screen.width / 20) / 2),
		                      mousePos.y -  ((Screen.width / 20) / 2),
		                      Screen.width / 20,
		                      Screen.width / 20), 
		                	  crosshairTexture);
	}


}
