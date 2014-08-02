using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	// Weapon mount points on turret (this is where lasers will be fired from)
	[SerializeField] Vector3[] weaponMountPoints;	
	// Laser shot prefab
	[SerializeField] Transform laserShotPrefab;
	// Laser shot sound effect
	[SerializeField] AudioClip soundEffectFire;
	// Object to track
	[SerializeField] Transform target;

	[SerializeField] Vector3 targetDirection; // Normalized direction.
	[SerializeField] float targetSpeed; //World units.
	
	[SerializeField] float projectileSpeed = 200.0f; //World units
	[SerializeField] float maxDegreeRotation = 30.0f;

	public float scaleLimit = 0.5f;
	public float z = 10.0f;
	

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
				if (!target)
						return;

				targetDirection = target.transform.forward;
				targetSpeed = target.rigidbody.velocity.magnitude;

				Vector3 leadDirection = target.position - transform.position;
				leadDirection += leadDirection.magnitude
						* targetDirection
						* targetSpeed
						/ projectileSpeed;
				transform.rotation = Quaternion.RotateTowards (transform.rotation,
		                                              Quaternion.LookRotation (leadDirection, transform.up),
		                                              maxDegreeRotation);
				//ShootRay ();

				if (Input.GetButton ("Fire3")) {
						// Itereate through each weapon mount point Vector3 in array
						foreach (Vector3 _wmp in weaponMountPoints) {
								// Calculate where the position is in world space for the mount point
								Vector3 _pos = transform.position + transform.right * _wmp.x + transform.up * _wmp.y + transform.forward * _wmp.z;
								// Instantiate the laser prefab at position with the spaceships rotation
				Vector3 direction = Random.insideUnitCircle * scaleLimit;
				direction.z = z; // circle is at Z units 
				direction = transform.TransformDirection( direction.normalized );    
								Transform _laserShot = (Transform)Instantiate (laserShotPrefab, _pos, transform.rotation);
								// Specify which transform it was that fired this round so we can ignore it for collision/hit
								_laserShot.GetComponent<SU_LaserShot> ().firedBy = transform;			
						}
						// Play sound effect when firing
						if (soundEffectFire != null) {
								audio.PlayOneShot (soundEffectFire);
				
						}
				}
		}

	void ShootRay() {
		//  Generate a random XY point inside a circle:

		Vector3 direction = Random.insideUnitCircle * scaleLimit;
		direction.z = z; // circle is at Z units 
		direction = transform.TransformDirection( direction.normalized );    
		//Raycast and debug
		Ray r = new Ray( transform.position, direction );
		RaycastHit hit;     
		if( Physics.Raycast( r, out hit ) ) {
			Debug.DrawLine( transform.position, hit.point ); 
		} 
	}
}
