// Planet C# Script (version: 1.02)
// SPACE UNITY - Space Scene Construction Kit
// http://www.spaceunity.com
// (c) 2013 Stefan Persson

// DESCRIPTION:
// Script for the rotational behaviour of planets.

// INSTRUCTIONS:
// This script is attached to the planet prefabs and rotation speed around its own axis can be configured.
// The SpaceSceneConstructionKit window will automatically configure random rotation speed.

// PARAMETERS:
//  planetRotation	(The rotational vector (axis and speed) of the planet)

// Version History
// 1.02 - Prefixed with SU_Planet to avoid naming conflicts.
// 1.01 - Initial Release.

using UnityEngine;
using System.Collections;

public class SU_Planet : MonoBehaviour {
	// Planet rotation vector specifying axis and rotational speed
	public Vector3 planetRotation;
	// Private variables
	private Transform _cacheTransform;
	
	void Start () {
		// Cache reference to transform to improve performance
		_cacheTransform = transform;	
	}
	
	void Update () {
		// Rotate the planet based on the rotational vector
		if (_cacheTransform != null) {			
			_cacheTransform.Rotate(planetRotation * Time.deltaTime);
		}
	}

}
