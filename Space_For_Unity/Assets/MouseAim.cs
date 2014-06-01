using UnityEngine;
using System.Collections;

public class MouseAim : MonoBehaviour {
	[SerializeField] Transform mouseAimObject;
	[SerializeField] Texture2D crosshairTexture;
	// Use this for initialization
	void Start () {
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
		
		mousePos.x = Mathf.Clamp(mousePos.x, 300, 1200);
		mousePos.y = Mathf.Clamp(mousePos.y, 150, 800);
		
		GUI.DrawTexture(new Rect( mousePos.x - (crosshairTexture.width/2),
		                      mousePos.y - (crosshairTexture.height/2),
		                      crosshairTexture.width,
		                      crosshairTexture.height), crosshairTexture);
	}


}
