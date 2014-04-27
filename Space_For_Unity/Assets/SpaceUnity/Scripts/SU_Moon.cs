// Moon C# Script (version: 1.02)
// SPACE UNITY - Space Scene Construction Kit
// http://www.spaceunity.com
// (c) 2013 Stefan Persson

// DESCRIPTION:
// Script for the rotational and orbiting behaviours of moons.

// INSTRUCTIONS:
// This script is attached to the moon prefabs and orbit speed and rotation speed around its own axis can be configured.
// The SpaceSceneConstructionKit window will automatically configure random orbit and rotation speed.

// PARAMETERS:
//  orbitSpeed		(The rotational speed of moon orbiting the planet)
//  rotationSpeed	(The rotational speed of moon around its own axis)

// HINTS:
// This script relies on the fact that the moon is actually a parent object at the same position as the parent planet.
// As the parent object rotates around its own axis, the child transform, "MoonObject", will orbit around the planet at its given distance
// from the parent.

// Version History
// 1.02 - Prefixed with SU_Moon to avoid naming conflicts.
// 1.01 - Initial Release.

using UnityEngine;
using System.Collections;

public class SU_Moon : MonoBehaviour {
	
	// Orbit speed of moon around its parent planet
	public float orbitSpeed = 0.0f;
	// Rotational speed of moon around its own acis
	public float rotationSpeed = 0.0f;	
	
	// Private Variables
	private Transform _cacheTransform;
	private Transform _cacheMeshTransform;
	
	void Start () {
		// Cache transforms to increase performance
		_cacheTransform = transform;
		_cacheMeshTransform = transform.Find("MoonObject");
	}
	
	void Update () {		
		// Orbit around the planet at orbitSpeed
		if (_cacheTransform != null) {
			_cacheTransform.Rotate(Vector3.up * orbitSpeed * Time.deltaTime);
		}
		
		// Rotate around own axis
		if (_cacheMeshTransform != null) {
			_cacheMeshTransform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
		}
	}
}
