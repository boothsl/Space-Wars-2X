using UnityEngine;
using System.Collections;

public class BaseCharacter : MonoBehaviour {

	public int team = 1;
	public int health = 100;

	public void DoDamage(int damage)
	{
		health -= damage;
		if (health <= 0)
		{
			//TODO: Explosions!
			Destroy(gameObject);
		}
	}
}
