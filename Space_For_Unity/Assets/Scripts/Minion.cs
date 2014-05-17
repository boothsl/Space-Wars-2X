using UnityEngine;
using System.Collections;

public class Minion : MonoBehaviour {

	private Spaceship _ship;

	// Use this for initialization
	void Start () {

		_ship = GetComponent<Spaceship>();

		// Start all thrusters when pressing Fire 1
		foreach (SU_Thruster _thruster in _ship.thrusters) {
			_thruster.StartThruster();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {

		Debug.Log("ENTER");
		Spaceship otherShip = other.GetComponent<Spaceship>();
		if (otherShip == null)
			return;

		if (otherShip.team == _ship.team)
			return;

		// ACTIVATE THE LASERS
		Debug.Log("FIRE!");
		_ship.FireMain();
	}
}
