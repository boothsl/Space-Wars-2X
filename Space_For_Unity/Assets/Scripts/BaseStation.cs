using UnityEngine;
using System.Collections;

public class BaseStation : BaseCharacter {

	public float spawnIntervalSeconds = 30;
	public BaseStation enemyBase;
	public BaseCharacter[] spawnPrefabs;
	public int spawnCount = 6;

	IEnumerator Start () {
		// Spawn minions in formation
		while (true)
		{
			for (int i = 0; i < spawnCount; i++)
			{
				BaseCharacter prefab = spawnPrefabs[Random.Range(0, spawnPrefabs.Length)];
				BaseCharacter newObj = Instantiate(prefab, 
				            transform.position + transform.forward * 200 
				            + new Vector3(Random.Range(-200, 200), Random.Range(-200, 200), 0),
				            transform.rotation) as BaseCharacter;
				newObj.team = this.team;

				Minion minion = newObj.GetComponent<Minion>();
				if (minion != null)
					minion.enemyBase = this.enemyBase;
			}

			yield return new WaitForSeconds(spawnIntervalSeconds);
		}
	}

	void OnDestroy() {
		GameController.Instance.GameOver();
	}
}
