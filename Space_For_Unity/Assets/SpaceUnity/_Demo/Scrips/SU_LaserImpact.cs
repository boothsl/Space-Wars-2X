// Laser Impact C# Script (version: 1.02)
// SPACE UNITY - Space Scene Construction Kit
// http://www.spaceunity.com
// (c) 2013 Stefan Persson

// DESCRIPTION:
// Small script to animate light intensity for impact effects.

// INSTRUCTIONS:
// This script is attached to the LaserImpact prefab for demo purposes.

// Version History
// 1.02 - Prefixed with SU_LaserImpact to avoid naming conflicts.
//        Added documentation. Removed unused variables. Introduced simple error checking.
// 1.01 - Initial Release.

using UnityEngine;
using System.Collections;

public class SU_LaserImpact : MonoBehaviour {
	// Cache light transform to improve performance
	public Transform _cacheLight;

	void Start () {
		// If the child light exists...
		if (transform.Find("light") != null) {
			// Cache the transform to improve performance
			_cacheLight = transform.Find("light");
			// Find the child light and set intensity to 1.0
			_cacheLight.light.intensity = 1.0f;
			// Move the transform 5 units out so it's not spawn at impact point of the collider/mesh it just hit
			// which will light up the object better.
			_cacheLight.transform.Translate(Vector3.up*5, Space.Self);

		} else {
			Debug.LogWarning("Missing required child light. Impact light effect won't be visible");
		}
		
	}
		
	void Update () {
		// If the light exists...
		if (_cacheLight != null) {
			// Set the intensity depending on the number of particles visible
			_cacheLight.light.intensity = (float) (transform.particleEmitter.particleCount / 50.0f);
		}
	}
}
