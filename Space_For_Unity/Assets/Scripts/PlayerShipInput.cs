using UnityEngine;
using System.Collections;

public class PlayerShipInput : MonoBehaviour {

	public bool is_top_player = true;

	private Spaceship _ship;
	
	void Start () {
		// Ensure that the thrusters in the array have been linked properly
		_ship = GetComponent<Spaceship>();
		if (_ship == null) {
			Debug.LogError("Spaceship component not found");
		}
	}
	
	void Update () {
		string fire1_string = "Fire1-bottom";
		string fire2_string = "Fire2-bottom";
		if (is_top_player) {
			fire1_string = "Fire1-top";
			fire2_string = "Fire2-top";
		}
		
		// Start all thrusters when pressing Fire 1
		if (Input.GetButtonDown(fire1_string)) {		
			foreach (SU_Thruster _thruster in _ship.thrusters) {
				_thruster.StartThruster();
			}
		}
		// Stop all thrusters when releasing Fire 1
		if (Input.GetButtonUp(fire1_string)) {		
			foreach (SU_Thruster _thruster in _ship.thrusters) {
				_thruster.StopThruster();
			}
		}
		
		if (Input.GetButtonDown(fire2_string)) {
			_ship.FireMain();
		}
		//Return to main menu on Escape (or other) key event
		if (Input.GetKey (KeyCode.Escape)) {
			//Application.LoadLevel(targetScene);
		}
	}

	void FixedUpdate () {
		string horizontal_string = "Horizontal-bottom";
		string vertical_string = "Vertical-bottom";
		if (is_top_player) {
			horizontal_string = "Horizontal-top";
			vertical_string = "Vertical-top";
		}

		_ship.PhysicsUpdate(Input.GetAxis(horizontal_string), Input.GetAxis(vertical_string));
		/*
		// In the physics update...
		// Add relative rotational roll torque when steering left/right
		_cacheRigidbody.AddRelativeTorque(new Vector3(0,0,-Input.GetAxis(horizontal_string)*rollRate*_cacheRigidbody.mass));
		// Add rudder yaw torque when steering left/right
		_cacheRigidbody.AddRelativeTorque(new Vector3(0,Input.GetAxis(horizontal_string)*yawRate*_cacheRigidbody.mass,0));
		// Add pitch torque when steering up/down
		_cacheRigidbody.AddRelativeTorque(new Vector3(Input.GetAxis(vertical_string)*pitchRate*_cacheRigidbody.mass,0,0));	*/
	}
}
