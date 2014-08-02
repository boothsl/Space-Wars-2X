/* commented out because I broke it, uses an older spaceship controller -Chris
 * 
 * using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Minion : MonoBehaviour {

	const float FIRE_RANGE = 250;

	public BaseCharacter enemyBase;

	private Spaceship _ship;

	private BaseCharacter _followTarget;
	private bool _isFiring = false;
	private Rigidbody _rigidBody;

	private List<BaseCharacter> enemies = new List<BaseCharacter>();

	// Use this for initialization
	void Start () {

		// Cache components
		_ship = GetComponent<Spaceship>();
		_rigidBody = GetComponent<Rigidbody>();

		// Start moving
		SetFollowTarget(enemyBase);
	}
	
	void Update () {

		if (_followTarget == null)
		{
			PickEnemy();
		}
		else
		{
			// Fire when within range
			Vector3 dir = _followTarget.transform.position - transform.position;
			if (Vector3.SqrMagnitude(dir) < FIRE_RANGE*FIRE_RANGE) {
				if (!_isFiring) {

					// Start firing, stop moving (ugly)
					_isFiring = true;
					//rigidbody.velocity = Vector3.zero;
					//_ship.StopThrusters();
					StartCoroutine(FireRoutine());
				}
			}
			else {
				_isFiring = false;
			}
		}
	}

	void FixedUpdate()
	{
		// Nudge other minions out of the way (psuedo-flocking behavior)
		Collider[] colliders = Physics.OverlapSphere(transform.position, 10);
		if (colliders != null)
		{
			foreach (Collider collider in colliders)
			{
				Spaceship other = collider.GetComponent<Spaceship>();
				if (other != null)
				{
					Vector3 dir = collider.transform.position - transform.position;
					other.rigidbody.AddForce(dir);
				}
			}
		}
	
		if (_followTarget != null)
		{
			Vector3 dir = _followTarget.transform.position - transform.position;

			if (!_isFiring) {
				// Move towards target
				_rigidBody.AddForce(dir.normalized * Time.fixedDeltaTime);

				// Clamp max velocity in the current direction
				const float MAX_VELOCITY = 100;
				if (_rigidBody.velocity.sqrMagnitude > MAX_VELOCITY*MAX_VELOCITY) {
					_rigidBody.velocity = _rigidBody.velocity.normalized * MAX_VELOCITY;
				}
			}

			// Rotate towards target, maximum 90 degrees/second
			transform.rotation = Quaternion.RotateTowards(
				transform.rotation, Quaternion.LookRotation(dir.normalized), Time.fixedDeltaTime * 90);
		}
	}

	IEnumerator FireRoutine() {

		while (_isFiring) {
			_ship.FireMain();
			yield return new WaitForSeconds(.5f + Random.Range(-.2f, .2f));
		}
	}

	public void SetFollowTarget(BaseCharacter target)
	{
		_followTarget = target;
		_isFiring = false;

		if (_followTarget != null)
			_ship.StartThrusters();
		else
			_ship.StopThrusters();

		Debug.Log("SetFollowTarget=" + _followTarget);
	}

	void PickEnemy()
	{
		// Clean enemies list
		for (int i = enemies.Count-1; i >= 0; i--)
		{
			if (enemies[i] == null)
				enemies.RemoveAt (i);
		}

		if (enemies.Count > 0)
		{
			// Pick a random enemy.
			// TODO, prefer current enemy, otherwise very close or attacking enemies, etc.
			SetFollowTarget(enemies[Random.Range(0, enemies.Count)]);
		}
		else if (enemyBase != null)
		{
			// Default to enemy base
			SetFollowTarget(enemyBase);
		}
	}

	void OnTriggerEnter(Collider other) {

		BaseCharacter otherShip = other.GetComponent<BaseCharacter>();
		if (otherShip == null) {
			// Check collider parent, for other ships
			if (other.transform.parent != null) {
				otherShip = other.transform.parent.GetComponent<BaseCharacter>();
			}
		}
		if (otherShip == null)
			return;
		
		if (otherShip.team == _ship.team)
			return;

		Debug.Log("ENTER " + otherShip);
		enemies.Add(otherShip);
		
		float currentTargetSqrDist = Mathf.Infinity;
		float newTargetSqrDist = Vector3.SqrMagnitude(otherShip.transform.position - transform.position);
		
		if (_followTarget != null)
		{
			currentTargetSqrDist = Vector3.SqrMagnitude(_followTarget.transform.position - transform.position);
		}
		if (_followTarget == null || newTargetSqrDist < currentTargetSqrDist)
		{
			SetFollowTarget(otherShip);
		}
	}

	void OnTriggerExit(Collider other) {

		BaseCharacter otherChar = other.GetComponent<BaseCharacter>();
		if (otherChar == null || otherChar.team == _ship.team)
			return;

		Debug.Log("EXIT " + otherChar);

		enemies.Remove(otherChar);
	}
}
*/