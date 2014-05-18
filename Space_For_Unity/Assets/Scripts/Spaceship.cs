

using UnityEngine;
using System.Collections;

public class Spaceship : BaseCharacter {

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
	
	// Private variables
	private Rigidbody _cacheRigidbody;
	
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
	}

	public void StartThrusters()
	{
		foreach (SU_Thruster thruster in thrusters) {
			thruster.StartThruster();
		}
	}

	public void StopThrusters()
	{
		foreach (SU_Thruster thruster in thrusters) {
			thruster.StopThruster();
		}
	}

	public void FireMain()
	{
		// Itereate through each weapon mount point Vector3 in array
		foreach (Vector3 _wmp in weaponMountPoints) {
			// Calculate where the position is in world space for the mount point
			Vector3 _pos = transform.position + transform.right * _wmp.x + transform.up * _wmp.y + transform.forward * _wmp.z;
			// Instantiate the laser prefab at position with the spaceships rotation
			Transform _laserShot = (Transform) Instantiate(laserShotPrefab, _pos, transform.rotation);
			// Specify which transform it was that fired this round so we can ignore it for collision/hit
			_laserShot.GetComponent<SU_LaserShot>().firedBy = transform;
		}

		// Play sound effect when firing
		if (soundEffectFire != null) {
			audio.PlayOneShot(soundEffectFire);	
		}
	}

	public void PhysicsUpdate(float horizontal, float vertical)
	{
		// In the physics update...
		// Add relative rotational roll torque when steering left/right
		_cacheRigidbody.AddRelativeTorque(new Vector3(0,0,-horizontal*rollRate*_cacheRigidbody.mass));
		// Add rudder yaw torque when steering left/right
		_cacheRigidbody.AddRelativeTorque(new Vector3(0,horizontal*yawRate*_cacheRigidbody.mass,0));
		// Add pitch torque when steering up/down
		_cacheRigidbody.AddRelativeTorque(new Vector3(vertical*pitchRate*_cacheRigidbody.mass,0,0));	
	}
}
