// Asteroid C# Script (version: 1.02)
// SPACE UNITY - Space Scene Construction Kit
// http://www.spaceunity.com
// (c) 2013 Stefan Persson

// DESCRIPTION:
// This script handles an asteroid in terms of rotation and movement.

// INSTRUCTIONS:
// Drag this script onto an asteroid (or use the existing Asteroid prefab) and configure properties.

// PARAMETERS:
//   rotationSpeed	(rotational speed of the asteroid)
//   driftSpeed		(drift/movement speed of the asteroid)

// Version History
// 1.02 - Prefixed with SU_Asteroid to avoid naming conflicts.
//        Added documentation.
// 1.01 - Initial Release.

using UnityEngine;
using System.Collections;

public class SU_Asteroid : MonoBehaviour {
	// Enum to present choise of high, medium, or low quality mesh
	public enum PolyCount { HIGH, MEDIUM, LOW }
	// Variable to set the poly count (quality) of the asteroid, defualt is High quality
	public PolyCount polyCount = PolyCount.HIGH;
	// Variable to set the poly count for the collider (MUCH faster to use the low poly version)
	public PolyCount polyCountCollider = PolyCount.LOW;
	
	// Link prefabs to the different quality meshes
	public Transform meshLowPoly;
	public Transform meshMediumPoly;
	public Transform meshHighPoly;
	
	// Rotation speed
	public float rotationSpeed = 0.0f;
	// Vector3 axis to rotate around
	public Vector3 rotationalAxis = Vector3.up;	
	// Drift/movement speed
	public float driftSpeed = 0.0f;
	// Vector3 direction for drift/movement
	public Vector3 driftAxis = Vector3.up;
	
	// Private variables
	private Transform _cacheTransform;
	
	void Start () {
		// Cache transforms to increase performance
		_cacheTransform = transform;
		// Set the mesh based on poly count (quality)
		SetPolyCount(polyCount);
	}
	
	void Update () {						
		if (_cacheTransform != null) {
			// Rotate around own axis
			_cacheTransform.Rotate(rotationalAxis * rotationSpeed * Time.deltaTime);
			// Move in world space according to drift speed
			_cacheTransform.Translate(driftAxis * driftSpeed * Time.deltaTime, Space.World);
		}
	}
	
	// Set the mesh based on the poly count (quality)
	public void SetPolyCount(PolyCount _newPolyCount) { SetPolyCount(_newPolyCount, false); }
	public void SetPolyCount(PolyCount _newPolyCount, bool _collider) {
		// If this is not the collider...
		if (!_collider) {
			// This is the actual asteroid mesh.. so specify which poly count we want
			polyCount = _newPolyCount;
			switch (_newPolyCount) {
			case PolyCount.LOW:
				// access the MeshFilter component and change the sharedMesh to the low poly version
				transform.GetComponent<MeshFilter>().sharedMesh = meshLowPoly.GetComponent<MeshFilter>().sharedMesh;				
				break;
			case PolyCount.MEDIUM:
				// access the MeshFilter component and change the sharedMesh to the medium poly version
				transform.GetComponent<MeshFilter>().sharedMesh = meshMediumPoly.GetComponent<MeshFilter>().sharedMesh;
				break;
			case PolyCount.HIGH:
				// access the MeshFilter component and change the sharedMesh to the high poly version
				transform.GetComponent<MeshFilter>().sharedMesh = meshHighPoly.GetComponent<MeshFilter>().sharedMesh;			
				break;
			}
		} else {
			// This is the collider mesh we set this time
			polyCountCollider = _newPolyCount;
			switch (_newPolyCount) {
			case PolyCount.LOW:
				// access the MeshFilter component and change the sharedMesh to the low poly version
				transform.GetComponent<MeshCollider>().sharedMesh = meshLowPoly.GetComponent<MeshFilter>().sharedMesh;				
				break;
			case PolyCount.MEDIUM:
				// access the MeshFilter component and change the sharedMesh to the medium poly version
				transform.GetComponent<MeshCollider>().sharedMesh = meshMediumPoly.GetComponent<MeshFilter>().sharedMesh;
				break;
			case PolyCount.HIGH:
				// access the MeshFilter component and change the sharedMesh to the high poly version
				transform.GetComponent<MeshCollider>().sharedMesh = meshHighPoly.GetComponent<MeshFilter>().sharedMesh;			
				break;
			}			
		}
	}
			
}
