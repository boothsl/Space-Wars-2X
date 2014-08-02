// Laser Shot C# Script (version: 1.02)
// SPACE UNITY - Space Scene Construction Kit
// http://www.spaceunity.com
// (c) 2013 Stefan Persson

// DESCRIPTION:
// Laser shot script for demo purposes. Moves the transform with a velocity and
// uses raycast to check for hit and spawn impact effect. It also randomly destroys
// objects it hits. The reason for randomness is just to allow impact effect and you
// would most likely want to implement a "hit points" script for targets where you
// can deduct hitpoints on impact until it is destroyed rather than using the random
// demo function in this script.

// This has been updated to support network multiplayer:
// - If this is not the server, then this is no raycasting.
// - If this is the server, then network then need to let the game controller know of
//   the hit so it can properly handle it.
// The game controller should implement IGameHitCounter and assign it to the
// property "GameHitController".

// INSTRUCTIONS:
// This script is attached to the LaserShot prefab for demo purposes.

// Version History
// 1.02 - Prefixed with SU_LaserShot to avoid naming conflicts.
//        Added documentation.
// 1.01 - Initial Release.

using UnityEngine;
using System.Collections;

public class LaserShot : MonoBehaviour {	
	// Default life of laser beam
	public float life = 2.0f;
	// Default velocity of laser beam
	public float velocity = 1000.0f;
	// Reference to impact effect prefab to spawn upon impact
	public Transform impactEffect;
	// Reference to explosion effect prefab to spawn if object is destroyed
	public Transform explosionEffect;
	// "Fired By" Reference to ignore collision detection for the ship that fired the laser
	public Transform firedBy {get; set;}

	// Set this to true if you want to process local "kills"
	public bool isLocal { get; set; }
	
	// Private variables
	private Vector3 _velocity;
	private Vector3 _newPos;
	private Vector3 _oldPos;	
	
	void Start () {
		// Set the new position to the current position of the transform
		_newPos = transform.position;
		// Set the old position to the same value
		_oldPos = _newPos;			
		// Set the velocity vector 3 to the specified velocity and set the direction to face forward of the transform
		_velocity = velocity * transform.forward;
		// Set the gameobject to destroy after period "life"
		Destroy(gameObject, life);
	}
	
	void Update () {
		// Change new position by the velocity magnitude (in the direction of transform.forward) and since
		// we are in the update function we need to multiply by deltatime.
		_newPos += transform.forward * _velocity.magnitude * Time.deltaTime;
		// SDet direction to the difference between new position and old position
		Vector3 _direction = _newPos - _oldPos;
		// Get the distance which is the magnitude of the direction vector
		float _distance = _direction.magnitude;
				
		// If distance is greater than nothing...
		if (_distance > 0) {
			// Define a RayCast
			RaycastHit _hit;
			// If the raycast from previous position in the specified direction at (or before) the distance...
			if (Physics.Raycast(_oldPos, _direction, out _hit, _distance)) {
				// and if the transform we hit isn't a the ship that fired the weapon and the collider isn't just a trigger...
				if (_hit.transform != firedBy && !_hit.collider.isTrigger) {
					// Set the rotation of the impact effect to the normal of the impact surface (we wan't the impact effect to
					// throw particles out from the object we just hit...
					Quaternion _rotation = Quaternion.FromToRotation(Vector3.up, _hit.normal);
					// Instantiate the imapct effect at impact position
					Instantiate(impactEffect, _hit.point, _rotation);

					if (!isLocal)
					{
						// Trigger the hit on the server
						GlobalGameState.OnHitObject(
							this, 
							new GlobalGameState.HitObjectArgs() { hitStrength = 1, hitRaycast = _hit, weaponObject = this.transform, explosionEffect = this.explosionEffect } );
					} else {
						// If random number is a small value...
						if (Random.Range(0,20) < 2) {
							// Instantiate the explosion effect at the point of impact
							Instantiate(explosionEffect, _hit.transform.position, _rotation);
							// Destroy the game object that we just hit
							Destroy(_hit.transform.gameObject);
						}
					}

					// Destroy the laser shot game object (on the client too)
					Destroy(gameObject);
				}
			}
		}
		// Set the old position tho the current position
		_oldPos = transform.position;
		// Set the actual position to the new position
		transform.position = _newPos;		
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		// Do nothing. We don't actually need to serialize since bullet motions are predictable
		// and whether they hit something is determined by the server.
		// However, Unity doesn't know anything about an object created by Network.Instantiate
		// unless it has a network view. That way the server has some way to know who
		// fired this shot.
	}
}
