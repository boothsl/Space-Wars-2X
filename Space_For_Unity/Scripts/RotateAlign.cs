using UnityEngine;
using System.Collections;

public class RotateAlign : MonoBehaviour {
	public GameObject lookAtObj;
	private float rotateSpeed = 10;

	// Update is called once per frame
	void Update () {
		transform.RotateAround (Vector3.zero, Vector3.up, rotateSpeed * Time.deltaTime);
	}
}
