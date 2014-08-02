// Spaceship C# Script (version: 1.02)
// SPACE UNITY - Space Scene Construction Kit
// http://www.spaceunity.com
// (c) 2013 Stefan Persson

// This version supports network-multiplayer

// DESCRIPTION:
// Thruster, steering and weapon control script for Spaceship prefab.

// INSTRUCTIONS:
// This script is attached to the Spaceship demo prefab. Configure parameters to suit your needs.

// PARAMETERS:
//  thrusters			(Thruster array containing reference to thrusters prefabs attached to the ship for propulsion)
//  rollRate			(multiplier for rolling the ship when steering left/right)	
//  yawRate				(multiplier for rudder/steering the ship when steering left/right)
//  pitchRate			(multiplier for pitch when steering up/down)
//  weaponMountPoints	(Vector3 array for mount points relative to ship where weapons will fire from)
//  laserShotPrefab		(reference to Laser Shot prefab, i.e. the laser bullet prefab to be instanitated)
//  soundEffectFire		(sound effect audio clip to be played when firing weapon)

// HINTS:
// The propulsion force of the thruster is configured for each attached thruster in the Thruster script.

// Version History
// 1.02 - Prefixed with SU_Spaceship to avoid naming conflicts.
// 1.01 - Initial Release.

using UnityEngine;
using System.Collections;

public class Spaceship : MonoBehaviour {
	
	public bool is_menu_ship;


	public string targetScene;
	// Array of thrusters attached to the spaceship
	public SU_Thruster[] thrusters;
	// Specify the roll rate (multiplier for rolling the ship when steering left/right)	
	public float rollRate = 100.0f;
	// Specify the yaw rate (multiplier for rudder/steering the ship when steering left/right)
	public float yawRate = 30.0f;
	// Specify the pitch rate (multiplier for pitch when steering up/down)
	public float pitchRate = 100.0f;
	// Weapon mount points on ship (this is where lasers will be fired from)
	public Vector3[] weaponMountPoints;	
	// Laser shot prefab
	public Transform laserShotPrefab;
	// Laser shot sound effect
	public AudioClip soundEffectFire;
	// Gap between auto-fire rounds.
	public float fireRate = 0.1f;
	
	// Private variables
	private Rigidbody _cacheRigidbody;
	private float	nextFire = 0.0f;
	
	
	void Start () {		
		// Ensure that the thrusters in the array have been linked properly
		foreach (SU_Thruster _thruster in thrusters) {
			if (_thruster == null) {
				Debug.LogError("Thruster array not properly configured. Attach thrusters to the game object and link them to the Thrusters array.");
			}			
		}
		
		// Cache reference to rigidbody to improve performance
		_cacheRigidbody = rigidbody;
		if (_cacheRigidbody == null) {
			Debug.LogError("Spaceship has no rigidbody - the thruster scripts will fail. Add rigidbody component to the spaceship.");
		}

		if (GlobalGameState.MainRadar != null)
		{
			if (networkView.isMine)
			{
				GlobalGameState.MainRadar.AddRadarBlip(
					this.gameObject,
					Color.yellow,
					0.5f);
			}
			else
			{
				GlobalGameState.MainRadar.AddRadarBlip(
					this.gameObject,
					Color.red,
					0.5f);
			}
		}
	}
	
	void Update () {

		// Is this me?
		if (!networkView.isMine && !is_menu_ship)
		{
			SyncedMovement();
			return;
		}

		string fire_string = "fire";
		string boost_string = "boost";
		string brake_string = "brake";

		// Stop all thrusters when pressing Fire 1
		if (Input.GetButtonDown(brake_string)) {		
			foreach (SU_Thruster _thruster in thrusters) {
				_thruster.StopThruster();
			}
		}
		// Start all thrusters when releasing Fire 1
		if (Input.GetButtonUp(brake_string)) {		
			foreach (SU_Thruster _thruster in thrusters) {
				_thruster.StartThruster();
			}
		}
		// Boost all thrusters when pressing Fire 3
		if (Input.GetButtonDown(boost_string)) {		
			foreach (SU_Thruster _thruster in thrusters) {
				_thruster.BoostThruster();
			}
		}
		// Unboost all thrusters when releasing Fire 3
		if (Input.GetButtonUp(boost_string)) {		
			foreach (SU_Thruster _thruster in thrusters) {
				_thruster.StartThruster();
			}
		}		
		if (Input.GetButton(fire_string) && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			// Itereate through each weapon mount point Vector3 in array
			foreach (Vector3 _wmp in weaponMountPoints) {
				// Calculate where the position is in world space for the mount point
				Vector3 _pos = transform.position + transform.right * _wmp.x + transform.up * _wmp.y + transform.forward * _wmp.z;

				// Instantiate the laser prefab at position with the spaceships rotation
				Transform _laserShot;
				if (!is_menu_ship) {
					_laserShot = (Transform) Network.Instantiate(laserShotPrefab, _pos, transform.rotation, 0);
				}
				else { 
					_laserShot = (Transform) GameObject.Instantiate(laserShotPrefab, _pos, transform.rotation);
				}
				// Specify which transform it was that fired this round so we can ignore it for collision/hit
				_laserShot.GetComponent<LaserShot>().firedBy = transform;
				
			}
			// Play sound effect when firing
			if (soundEffectFire != null) {
				audio.PlayOneShot(soundEffectFire);

			}
		}
	}

	void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		rigidbody.position = Vector3.Lerp (syncStartPosition, syncEndPosition, syncTime/syncDelay);
	}

	void FixedUpdate () {

		if (!networkView.isMine && !is_menu_ship)
			return;

		string horizontal_string = "horizontal";
		string vertical_string = "vertical";

		float horizontal = Input.GetAxis(horizontal_string);
		float vertical = Input.GetAxis (vertical_string);
		// In the physics update...
		// Add relative rotational roll torque when steering left/right
		_cacheRigidbody.AddRelativeTorque(new Vector3(0,0,-horizontal*rollRate*_cacheRigidbody.mass));
		// Add rudder yaw torque when steering left/right
		_cacheRigidbody.AddRelativeTorque(new Vector3(0,horizontal*yawRate*_cacheRigidbody.mass,0));
		// Add pitch torque when steering up/down
		_cacheRigidbody.AddRelativeTorque(new Vector3(vertical*pitchRate*_cacheRigidbody.mass,0,0));	
	}

	private float lastSynchronizationTime = 0.0f;
	private float syncDelay = 0.0f;
	private float syncTime = 0.0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		Vector3 syncVelocity = Vector3.zero;
		Quaternion rotation = Quaternion.identity;
		if (stream.isWriting)
		{
			syncPosition = rigidbody.position;
			stream.Serialize(ref syncPosition);
			
			syncVelocity = rigidbody.velocity;
			stream.Serialize(ref syncVelocity);

			rotation = rigidbody.rotation;
			stream.Serialize(ref rotation);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncVelocity);
			stream.Serialize(ref rotation);
			//stream.Serialize(ref forward);
			//stream.Serialize(ref up);
			
			syncTime = 0.0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
			
			syncEndPosition = syncPosition + syncVelocity*syncDelay;
			syncStartPosition = rigidbody.position;

			rigidbody.rotation = rotation;
		}
	}

	void OnDestroy()
	{
		if (networkView.isMine)
		{
			// Invoke the event on the controller to respawn me.
			GlobalGameState.OnPlayerDied (this, new System.EventArgs());
		}

		// Remove this object from the radar, it will be re-added with a respawn
		if (GlobalGameState.MainRadar != null)
		{
			GlobalGameState.MainRadar.RemoveRadarBlip(gameObject);
		}
	}
}
