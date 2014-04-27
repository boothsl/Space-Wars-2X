using UnityEngine;
using System.Collections;

public class SU_Explosion : MonoBehaviour {
	public float destroyAfterSeconds = 8.0f;

	void Awake() {
		// Destroy gameobject after delay
		Destroy(gameObject, destroyAfterSeconds);
	}
}
