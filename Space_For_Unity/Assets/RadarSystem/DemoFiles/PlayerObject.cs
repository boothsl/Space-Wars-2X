using UnityEngine;
using System.Collections;

public class PlayerObject : MonoBehaviour {
	
	// Create Inspector Reference to RadarSystem - Drag and Drop on RadarSystem to Inspector Variable
	public RadarSystem _radarSystem;
	
	// Use this for initialization
	void Start ()
	{
		if (_radarSystem == null){
			Debug.Log( "Please add the RadarSystem Script to this Object");
		} else {
			//Add Self to RadarSystem
			_radarSystem.AddRadarBlip (this.gameObject, Color.green, 1.0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
