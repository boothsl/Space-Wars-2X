// Thruster C# Script (version: 1.02)
// SPACE UNITY - Space Scene Construction Kit
// http://www.spaceunity.com
// (c) 2013 Stefan Persson

// DESCRIPTION:
// This script controls the thruster used for spaceship propulsion.

// INSTRUCTIONS:
// Use the thruster prefab which has the required particle system this script depends upon.
// Configure the thruster force and other parameters as desired.

// PARAMETERS:
//   thrusterForce		(the thruster force to be applied to it's rigidbody parent when active
//   addForceAtPosition	(whether or not to introduce torque/rotation... use with care as it is super sensitive for positioning)
//   soundEffectVolume	(sound effect volume of thruster)

// Version History
// 1.02 - Prefixed with SU_Thruster to avoid naming conflicts.
// 1.01 - Initial Release.

using UnityEngine;
using System.Collections;

public class SU_Thruster : MonoBehaviour {
	// Force of individual thrusters
	public float thrusterForce = 10000;
	// Whether or not to add force at position which introduces torque, use with care...
	public bool addForceAtPosition = false;
	// Sound effect volume of thruster
	public float soundEffectVolume = 1.0f;
	
	// Private variables
	private bool _isActive = false;	
	private Transform _cacheTransform;
	private Rigidbody _cacheParentRigidbody;
	private Light _cacheLight;
	private ParticleSystem _cacheParticleSystem;
	
	// Call StartThruster() function from other scripts to start the thruster
	public void StartThruster() {
		// Set the thruster active flag to true
		_isActive = true; 
	}
	
	// Call StopThruster() function from other scripts to stop the thruster
	public void StopThruster() {
		// Set the thruster active flag to false		
		_isActive = false; 
	}
	
	void Start () {
		// Cache the transform and parent rigidbody to improve performance
		_cacheTransform = transform;
		
		// Ensure that parent object (e.g. spaceship) has a rigidbody component so it can apply force.
		if (transform.parent.rigidbody != null) {			
			_cacheParentRigidbody = transform.parent.rigidbody;
		} else {
			Debug.LogError("Thruster has no parent with rigidbody that it can apply the force to.");
		}
		
		// Cache the light source to improve performance (also ensure that the light source in the prefab is intact.)
		_cacheLight = transform.GetComponent<Light>().light;
		if (_cacheLight == null) {
			Debug.LogError("Thruster prefab has lost its child light. Recreate the thruster using the original prefab.");
		}
		// Cache the particle system to improve performance (also ensure that the particle system in the rpefab is intact.)
		_cacheParticleSystem = particleSystem;
		if (_cacheParticleSystem == null) {
			Debug.LogError("Thruster has no particle system. Recreate the thruster using the original prefab.");
		}
		
		// Start the audio loop playing but mute it. This is to avoid play/stop clicks and clitches that Unity may produce.
		audio.loop = true;
		audio.volume = soundEffectVolume;
		audio.mute = true;
		audio.Play();		
	}	
	
	void Update () {
		// If the light source of the thruster is intact...
		if (_cacheLight != null) {
			// Set the intensity based on the number of particles
			_cacheLight.intensity = _cacheParticleSystem.particleCount / 20;
		}
		
		// If the thruster is active...
		if (_isActive) {
			// ...and if audio is muted...
			if (audio.mute) {
				// Unmute the audio
				audio.mute=false;
			}
			// If the audio volume is lower than the sound effect volume...
			if (audio.volume < soundEffectVolume) {
				// ...fade in the sound (to avoid clicks if just played straight away)
				audio.volume += 5f * Time.deltaTime;
			}
			
			// If the particle system is intact...
			if (_cacheParticleSystem != null) {	
				// Enable emission of thruster particles
				_cacheParticleSystem.enableEmission = true;
			}		
		} else {
			// The thruster is not active
			if (audio.volume > 0.01f) {
				// ...fade out volume
				audio.volume -= 5f * Time.deltaTime;	
			} else {
				// ...and mute it when it has faded out
				audio.mute = true;
			}
			
			// If the particle system is intact...
			if (_cacheParticleSystem != null) {				
				// Stop emission of thruster particles
				_cacheParticleSystem.enableEmission = false;				
			}
			
		}
	
	}
	
	void FixedUpdate() {
		// If the thruster is active...
		if (_isActive) {
			// ...add the relative thruster force to the parent object
			if (addForceAtPosition) {
				// Add force relative to the position on the parent object which will also apply rotational torque
				_cacheParentRigidbody.AddForceAtPosition (_cacheTransform.up * thrusterForce, _cacheTransform.position);
			} else {
				// Add force without rotational torque
				_cacheParentRigidbody.AddRelativeForce (Vector3.forward * thrusterForce);				
			}
		}		
	}
}
