using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {
	public Transform lookAtObj;
	public float rotateSpeed = 10;

	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (lookAtObj.position, Vector3.up, rotateSpeed * Time.deltaTime);
		if (Input.anyKeyDown) {
			SU_CameraFollow followScript = (SU_CameraFollow)GetComponent(typeof(SU_CameraFollow));
			followScript.enabled = true;
			RotateCamera rotateScript = (RotateCamera)GetComponent(typeof(RotateCamera));
			rotateScript.enabled = false;

		}
	}
}