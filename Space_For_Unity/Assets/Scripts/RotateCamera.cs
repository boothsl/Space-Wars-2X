using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {
	public Transform lookAtObj;
	public float rotateSpeed = 10;

	/*public SU_Thruster thrustScript;

	void Start () {
		thrustScript = gameObject.Find("ThrusterLeft").GetComponent<SU_Thruster>();
		thrustScript.enabled = false;
	}*/
	// Update is called once per frame
	void Update () {
		transform.RotateAround (lookAtObj.position, Vector3.up, rotateSpeed * Time.deltaTime);
		if (Input.anyKeyDown) {
			CameraFollow followScript = (CameraFollow)GetComponent(typeof(CameraFollow));
			followScript.enabled = true;
			RotateCamera rotateScript = (RotateCamera)GetComponent(typeof(RotateCamera));
			rotateScript.enabled = false;
			//thrustScript.enabled = true;

		}
	}
}