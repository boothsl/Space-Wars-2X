using UnityEngine;
using System.Collections;

public class RadarTracker : MonoBehaviour {

	public RadarSystem _RadarSystem;

	void Start()
	{
		_RadarSystem.AddRadarBlip(this.gameObject, Color.red, 0.5f);
	}
}
