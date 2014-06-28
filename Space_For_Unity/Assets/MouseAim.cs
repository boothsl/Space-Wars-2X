using UnityEngine;
using System.Collections;

public class MouseAim : MonoBehaviour {
	[SerializeField] Transform mouseAimObject;
	[SerializeField] Texture2D crosshairTexture;

	[SerializeField] Vector3 targetDirection; // Normalized direction.
	[SerializeField] float targetSpeed; //World units.
	
	[SerializeField] float projectileSpeed = 200.0f; //World units
	[SerializeField] float maxDegreeRotation = 30.0f;

	public bool paused;
	// Use this for initialization
	void Start () {
		Screen.showCursor = false;
	}

	// Update is called once per frame
	void Update () {
				if (!paused)
						return;
			
				targetDirection = mouseAimObject.transform.forward;
				targetSpeed = mouseAimObject.rigidbody.velocity.magnitude;

				Vector3 leadDirection = mouseAimObject.position - transform.position;
				leadDirection += leadDirection.magnitude
						* targetDirection
						* targetSpeed
						/ projectileSpeed;
				transform.rotation = Quaternion.RotateTowards (transform.rotation,
		                                              Quaternion.LookRotation (leadDirection, transform.up),
		                                              maxDegreeRotation);	// Update is called once per frame
//	void Update () {
//		Vector3 mousePos = Input.mousePosition;
//		Vector3 wantedPos = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, 1000.0f));
//		transform.position = wantedPos;
//		transform.parent.position.MoveTowards(transform.position, wantedPos, 1.0f);
//	}
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

	void OnApplicationFocus(bool focusStatus) {
			paused = focusStatus;
		}

}
